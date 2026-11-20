using System;
using System.Net;

namespace SimplePocketBase
{
    public class PocketBaseException : Exception
    {
        public HttpStatusCode StatusCode { get; private set; }
        public string ResponseBody { get; private set; }

        public PocketBaseException(string message, HttpStatusCode statusCode, string responseBody) : base(message)
        {
            StatusCode = statusCode;
            ResponseBody = responseBody;
        }
    }
}
