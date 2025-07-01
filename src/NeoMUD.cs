using Serilog;
using Quartz;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using SuperSocket.Server.Host;
using SuperSocket.ProtoBase;
using System.Text;
using SuperSocket.Server.Abstractions;

namespace NeoMUD.src;

public class NeoMUD
{
  public static async Task Main()
  {
    DateTime startupTime = DateTime.Now;
    CancellationTokenSource cts = new();

    var builder = SuperSocketHostBuilder.Create<StringPackageInfo, CommandLinePipelineFilter>();

    Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console(restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Warning)
            .WriteTo.File("logs/debug.log", rollingInterval: RollingInterval.Hour)
            .WriteTo.SQLite("data.db", storeTimestampInUtc: true)
            .CreateLogger();

    builder.UseSerilog(Log.Logger);

    builder.ConfigureServices((hostContext, services) =>
    {
      services.AddQuartz(q =>
      {
        DoJobSetup(q);
      });
      services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
    });
    
    builder.UseSessionHandler(async (s) => 
        {
          await s.SendAsync(Encoding.UTF8.GetBytes(ConnectionSplash() + "\r\n"));
        },
        async (s,e) => 
        {
          // do nothing
        });
    builder.UsePackageHandler(async (session, package) => {
      var result = "";

        switch (package.Key.ToUpper()){
        case "PING":
          result = "PONG";
          break;
        default:
          result = "UNK";
          break;
        }

      await session.SendAsync(Encoding.UTF8.GetBytes(result + "\r\n"));
      });

    builder.ConfigureSuperSocket(opts => {
        opts.Name = "NeoMUD alpha1 (telnet-test)";
        opts.Listeners = new List<ListenOptions>{new ListenOptions{Ip = "Any", Port = 4040}};
        });

    var host = builder.Build();

    var lifetime = host.Services.GetRequiredService<IHostApplicationLifetime>();
    lifetime.ApplicationStarted.Register(ShowStartupSplash);
    lifetime.ApplicationStopped.Register(ShowExitSplash);


    await host.RunAsync();

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

  public static string ConnectionSplash(){
    return $"""
###############################################################################
###############################################################################
###  ######  ####          #####        #######################################
###   #####  ####  ############  ######  ######################################
###    ####  ####  ############  ######  ######################################
###  #  ###  ####  ############  ######  ######################################
###  ##  ##  ####       #######  ######  ######################################
###  ###  #  ####  ############  ######  ######################################
###  ####    ####  ############  ######  ######################################
###  ######  ####          #####        #######################################
###############################################################################
###############################################################################
#####################################  #######  ####  ######  ####         ####
#####################################   #####   ####  ######  ####  #####   ###
#####################################    ###    ####  ######  ####  ######  ###
#####################################  #  #  #  ####  ######  ####  ######  ###
#####################################  ##   ##  ####  ######  ####  ######  ###
#####################################  #######  ####  ######  ####  ######  ###
#####################################  #######  #####  ####  #####  #####   ###
#####################################  #######  ######      ######         ####
###############################################################################
###############################################################################
###############################################################################
###############################################################################
""";
  }

  public static void DoJobSetup(IServiceCollectionQuartzConfigurator q)
  {

  }



  public static void TickServer(CancellationToken ct,
                                ref List<string> runningThreadList)
  {
    runningThreadList.Add("TickServer");
    Log.Information("Started server tick thread");
    while (!ct.IsCancellationRequested)
    {
      // do server ticking logic here
    }

    // following code runs after cancellation requested
    Log.Information("Stopping server tick thread...");
    runningThreadList.Remove("TickServer");
  }
}
