using System.Net;

namespace Community.API.Utilities.Exceptions
{
    public class HttpInternalServerErrorException : HttpException
    {
        public override HttpStatusCode StatusCode { get; } = HttpStatusCode.InternalServerError;

        public HttpInternalServerErrorException(string message = "Internal Server Error!") : base(message) { }
    }
}
