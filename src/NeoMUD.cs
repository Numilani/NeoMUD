using Serilog;
using Quartz;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using SuperSocket.Server.Host;
using SuperSocket.ProtoBase;
using SuperSocket.Server.Abstractions;
using NeoMUD.src.Jobs;
using Microsoft.EntityFrameworkCore;
using NeoMUD.src.Services;

namespace NeoMUD.src;

public class NeoMUD
{
  public static async Task Main()
  {
    DateTime startupTime = DateTime.Now;
    CancellationTokenSource cts = new();

    var builder = SuperSocketHostBuilder.Create<StringPackageInfo, CommandLinePipelineFilter>();

    SetupSupportingServices(builder);
    SetupSocketConfig(builder);

    var host = builder.Build();

    await DoJobSetupAsync(await host.Services.GetRequiredService<ISchedulerFactory>().GetScheduler());

    var lifetime = host.Services.GetRequiredService<IHostApplicationLifetime>();
    lifetime.ApplicationStarted.Register(ShowStartupSplash);
    lifetime.ApplicationStopped.Register(ShowExitSplash);

    await host.RunAsync();

  }

  public static void SetupSupportingServices(SuperSocket.Server.Abstractions.Host.ISuperSocketHostBuilder<StringPackageInfo> builder)
  {
    Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            // .WriteTo.Console(restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Warning)
            .WriteTo.Console()
            .WriteTo.File("logs/debug.log", rollingInterval: RollingInterval.Hour)
            .WriteTo.SQLite("data.db", storeTimestampInUtc: true)
            .CreateLogger();

    builder.UseSerilog(Log.Logger);

    builder.ConfigureServices((hostContext, services) =>
    {
      services.AddDbContext<AppDbContext>(o =>
      {
        o.UseSqlite("Data Source=neomud.db");
      });
      services.AddScoped<UserService>();
      services.AddScoped<CharacterService>();
      services.AddScoped<RoomService>();
      services.AddScoped<ViewManager>();
      services.AddQuartz();
      services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
    });
  }

  public static void SetupSocketConfig(SuperSocket.Server.Abstractions.Host.ISuperSocketHostBuilder<StringPackageInfo> builder)
  {
    builder.UseSession<GameSession>();
    builder.UseSessionHandler(async (s) =>
          {
            await ((GameSession)s).CurrentView.Display();
          },
          async (s, e) =>
          {
            // do nothing
          });
    builder.UsePackageHandler(async (session, package) =>
    {
      await ((GameSession)session).CurrentView.ReceiveInput(package);
    });
    // builder.UseCommand((cmdOpts) =>
    //  {
    //    cmdOpts.AddCommandAssembly(Assembly.GetExecutingAssembly());
    //    cmdOpts.RegisterUnknownPackageHandler<StringPackageInfo>(async (session, pkg, ct) => { 
    //        var gs = (GameSession)session;
    //        if (gs.AwaitingInput){
    //         gs.StringInput = $"{pkg.Key} ${pkg.Body}";
    //        }
    //        else await session.SendTelnetStringAsync("Unknown command");
    //        });
    //  });
    builder.ConfigureSuperSocket(opts =>
    {
      opts.Name = "NeoMUD alpha1 (telnet-test)";
      opts.Listeners = new List<ListenOptions> { new ListenOptions { Ip = "Any", Port = 4040 } };
    });
  }

  public static async Task DoJobSetupAsync(IScheduler scheduler)
  {
    // set up jobs
    await scheduler.ScheduleJob(
        JobBuilder.Create<TickServerJob>()
          .WithIdentity("TickServer", "processing")
          .Build(),
          TriggerBuilder.Create()
            .WithSimpleSchedule(x => x
              .WithIntervalInMinutes(1)
              .RepeatForever())
            .Build()
        );
  }

  public static void ShowStartupSplash()
  {
    var startupTimeString21 = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss  ");
    Console.WriteLine($"""
===============================================================================
                             NeoMUD 0.0.1 Alpha
Starting At:                                                  Stop server with:
{startupTimeString21}                                                   CTRL-C
===============================================================================
""");
  }

  public static void ShowExitSplash()
  {
    var stoppedTimeString21 = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss  ");
    Console.WriteLine($"""
===============================================================================
                            NeoMUD shutting down
Stopping At:                                                 Graceful Shutdown:
{stoppedTimeString21}                                               SUCCESSFUL
===============================================================================
""");
  }


}
