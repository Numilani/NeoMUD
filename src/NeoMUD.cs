using Serilog;

namespace NeoMUD.src;

public class NeoMUD {
  public static void Main() {
    List<string> RunningThreadList = [];
    DateTime startupTime = DateTime.Now;
    CancellationTokenSource cts = new();

    // initialize background services
    Log.Logger =
        new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo
            .File("logs/debug.log", rollingInterval: RollingInterval.Hour)
            .WriteTo.SQLite("data.db", storeTimestampInUtc: true)
            .CreateLogger();

    DisplayStartupSplash(startupTime);

    // Spin up background threads
    Thread serverTickHandler =
        new(() => TickServer(cts.Token, ref RunningThreadList));

    serverTickHandler.Start();

    // Add handler for graceful shutdown on Ctrl-C
    Console.CancelKeyPress += (sender, e) => {
      e.Cancel = true;
      InitiateShutdown(cts, ref RunningThreadList);
    };
  }

  public static void DisplayStartupSplash(DateTime startTime) {
    var startupTimeString21 = startTime.ToString("yyyy-MM-dd HH:mm:ss  ");
    Console.WriteLine($"""
===============================================================================
                             NeoMUD 0.0.1 Alpha
Starting At:                                                  Stop server with:
{startupTimeString21}                                                   CTRL-C
===============================================================================
""");
  }

  public static void InitiateShutdown(CancellationTokenSource cts,
                                      ref List<string> runningThreadList) {
    cts.Cancel();
    while (runningThreadList.Count > 0) {
      // do nothing
    }
    var stoppedTimeString21 = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss  ");
    Console.WriteLine($"""
===============================================================================
                            NeoMUD shutting down
Stopping At:                                                 Graceful Shutdown:
{stoppedTimeString21}                                               SUCCESSFUL
===============================================================================
""");
  }

  public static void TickServer(CancellationToken ct,
                                ref List<string> runningThreadList) {
    runningThreadList.Add("TickServer");
    Log.Information("Started server tick thread");
    while (!ct.IsCancellationRequested) {
      // do server ticking logic here
    }

    // following code runs after cancellation requested
    Log.Information("Stopping server tick thread...");
    runningThreadList.Remove("TickServer");
  }
}
