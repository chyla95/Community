using System.Net;

namespace Community.API.Utilities.Exceptions
{
    public class HttpNotFoundException : HttpException
    {
        public override HttpStatusCode StatusCode { get; } = HttpStatusCode.NotFound;

        public HttpNotFoundException(string message = "Not Found!") : base(message) { }
    }
}
