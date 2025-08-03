using Microsoft.Extensions.Logging;
using Quartz;

namespace NeoMUD.src.Jobs;

public class TickServerJob : IJob
{
  private ILogger<TickServerJob> _logger;

    public TickServerJob(ILogger<TickServerJob> logger)
    {
      _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
      _logger.LogInformation("Server Ticking...");
    }
}
