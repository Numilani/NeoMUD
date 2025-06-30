using Serilog;
using Quartz;
using Microsoft.Extensions.Hosting;

namespace NeoMUD.src;

public class NeoMUD
{
  public static async Task Main()
  {
    DateTime startupTime = DateTime.Now;
    CancellationTokenSource cts = new();

    var builder = Host.CreateDefaultBuilder();

    builder.UseSerilog((ctx, services, config) =>
    {
      config.ReadFrom.Configuration(ctx.Configuration);
    });

    builder.ConfigureServices((hostContext, services) =>
    {
      services.AddQuartz(q => {
          // add jobs here
          });
      services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
    });

    var host = builder.Build();

    await host.RunAsync();


    List<string> RunningThreadList = [];

    // initialize background services
    Log.Logger =
        new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo
            .File("logs/debug.log", rollingInterval: RollingInterval.Hour)
            .WriteTo.SQLite("data.db", storeTimestampInUtc: true)
            .CreateLogger();

    // DisplayStartupSplash(startupTime);

    // Spin up background threads
    Thread serverTickHandler =
        new(() => TickServer(cts.Token, ref RunningThreadList));

    serverTickHandler.Start();

    // Add handler for graceful shutdown on Ctrl-C
    Console.CancelKeyPress += (sender, e) =>
    {
      e.Cancel = true;
      // InitiateShutdown(cts, ref RunningThreadList);
    };
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
