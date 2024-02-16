using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace CloudRepublic.BenchMark.API.V2;

public class Precache
{
    private readonly ILogger _logger;

    public Precache(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<Precache>();
    }

    [Function("Precache")]
    public void Run([TimerTrigger("0 15 * * * *")] TimerInfo myTimer)
    {
        _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

        if (myTimer.ScheduleStatus is not null)
        {
            _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
            
        }
    }
}