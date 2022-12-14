using System.Net;

namespace Community.API.Utilities.Exceptions
{
    public class HttpForbiddenException : HttpException
    {
        public override HttpStatusCode StatusCode { get; } = HttpStatusCode.Forbidden;

        public HttpForbiddenException(string message = "Forbidden!") : base(message) { }
    }
}
