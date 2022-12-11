using System.Net;

namespace Community.API.Utilities.Exceptions
{
    public class HttpBadRequestException : HttpException
    {
        public override HttpStatusCode StatusCode { get; } = HttpStatusCode.BadRequest;

        public HttpBadRequestException(string message = "Bad Request!") : base(message) { }
    }
}
