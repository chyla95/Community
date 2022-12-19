using System.Net;
using System.Text.Json;

namespace Community.API.Utilities.Exceptions
{
    public class HttpExceptionFieldMessage : HttpExceptionMessage
    {
        public string Field { get; }

        public HttpExceptionFieldMessage(string exception, string field) : base(exception)
        {
            Field = field;
        }
    }

    public class HttpValidationException : HttpException
    {
        public override HttpStatusCode StatusCode { get; } = HttpStatusCode.BadRequest;
        public override IEnumerable<HttpExceptionFieldMessage> Exceptions { get; }

        public HttpValidationException(IEnumerable<HttpExceptionFieldMessage> exceptions) : base("Request validation error!")
        {
            Exceptions = exceptions;
        }

        public override string SerializeToJson()
        {
            var schema = new { Exceptions };
            string serializedExceptions = JsonSerializer.Serialize(schema, jsonSerializerOptions);
            return serializedExceptions;
        }
    }
}
