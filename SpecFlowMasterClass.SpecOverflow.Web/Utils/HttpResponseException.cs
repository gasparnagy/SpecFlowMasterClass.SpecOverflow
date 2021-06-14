using System;
using System.Net;

namespace SpecFlowMasterClass.SpecOverflow.Web.Utils
{
    public class HttpResponseException : Exception
    {
        private readonly string _message = null;
        public int Status { get; set; } = 500;
        public object Value { get; set; }

        public override string Message =>
            _message ?? $"{Status}: {Value}";

        public HttpStatusCode StatusCode => (HttpStatusCode) Status;

        public HttpResponseException()
        {
        }

        public HttpResponseException(HttpStatusCode statusCode, object value = null, string message = null)
        {
            Value = value;
            Status = (int)statusCode;
            _message = message;
        }
    }
}
