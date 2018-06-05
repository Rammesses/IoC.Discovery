using Microsoft.AspNetCore.Http;
using System;

namespace SampleServices
{
    public class MyCustomService
    {
        private IHttpContextAccessor contextAccessor;

        public MyCustomService(IHttpContextAccessor contextAccessor)
        {
            this.contextAccessor = contextAccessor;
        }

        public string GetMessage()
        {
            var context = contextAccessor.HttpContext;

            return $"Hello {context.TraceIdentifier}";
        }
    }
}
