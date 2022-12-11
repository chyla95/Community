using System.Net;

namespace Community.API.Utilities.Exceptions
{
    public class HttpExceptionMessage
    {
        public string Exception { get; set; }

        public HttpExceptionMessage(string message)
        {
            Exception = message;
        }
    }

    public abstract class HttpException : Exception
    {
        public abstract HttpStatusCode StatusCode { get; }
        public IEnumerable<HttpExceptionMessage> Exceptions { get; set; }

        public HttpException(string message) : base(message)
        {
            Exceptions = new List<HttpExceptionMessage>
            {
                new HttpExceptionMessage(message)
            };
        }
    }
}
