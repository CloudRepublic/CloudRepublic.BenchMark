using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace CloudRepublic.BenchMark.Test.Helpers
{
    public class MockILoggerFactory
    {
        public static ILogger CreateLogger()
        {
            return NullLoggerFactory.Instance.CreateLogger("Null Logger");
        }
    }
}
