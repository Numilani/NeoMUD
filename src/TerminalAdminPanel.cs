using Microsoft.Extensions.Hosting;

public class TerminalAdminPanel : BackgroundService
{
  public override Task StartAsync(CancellationToken cancellationToken)
  {
    var startupTimeString21 = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss  ");
    Console.WriteLine($"""
===============================================================================
                             NeoMUD 0.0.1 Alpha
Starting At:                                                  Stop server with:
{startupTimeString21}                                                   CTRL-C
===============================================================================
""");

    return Task.CompletedTask;
  }
  public override Task StopAsync(CancellationToken cancellationToken)
  {
    var stoppedTimeString21 = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss  ");
    Console.WriteLine($"""
===============================================================================
                            NeoMUD shutting down
Stopping At:                                                 Graceful Shutdown:
{stoppedTimeString21}                                               SUCCESSFUL
===============================================================================
""");

    return Task.CompletedTask;
  }

  protected override Task ExecuteAsync(CancellationToken stoppingToken)
  {
    throw new NotImplementedException();
  }
}
