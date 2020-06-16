using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;

namespace CloudRepublic.BenchMark.Test.Helpers
{
    public class MockHttpRequestFactory
    {
        public static DefaultHttpRequest CreateHttpRequest(Dictionary<string, StringValues> queryParams)
        {
            return new DefaultHttpRequest(new DefaultHttpContext())
            {
                Query = new QueryCollection(queryParams)
            };
        }

        public static DefaultHttpRequest CreateHttpRequest()
        {
            return new DefaultHttpRequest(new DefaultHttpContext());
        }

    }
}
