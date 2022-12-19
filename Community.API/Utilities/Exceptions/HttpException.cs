using System.Net;
using System.Text.Json;

namespace Community.API.Utilities.Exceptions
{
    public class HttpExceptionMessage
    {
        public string Exception { get; }

        public HttpExceptionMessage(string exception)
        {
            Exception = exception;
        }
    }

    public abstract class HttpException : Exception
    {
        protected readonly JsonSerializerOptions jsonSerializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public abstract HttpStatusCode StatusCode { get; }
        public virtual IEnumerable<HttpExceptionMessage> Exceptions { get; }

        public HttpException(string message) : base(message)
        {
            Exceptions = new List<HttpExceptionMessage>
            {
                new HttpExceptionMessage(message)
            };
        }

        public virtual string SerializeToJson()
        {
            var schema = new { Exceptions };
            string serializedExceptions = JsonSerializer.Serialize(schema, jsonSerializerOptions);
            return serializedExceptions;
        }
    }
}
