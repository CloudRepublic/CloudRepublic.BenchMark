using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace CloudRepublic.BenchMark.Tests.Infrastructure
{
    public class TestFactory
    {
        public static DefaultHttpRequest CreateHttpRequest(string queryKey,string queryValue)
        {
            return new DefaultHttpRequest(new DefaultHttpContext())
            {
                Query = new QueryCollection(new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>() { { queryKey, queryValue} })
            };
        }

        public static DefaultHttpRequest CreateHttpRequest()
        {
            return new DefaultHttpRequest(new DefaultHttpContext());
        }


        public static ILogger CreateLogger()
        {
            return NullLoggerFactory.Instance.CreateLogger("Null Logger");
        }
    }
}
