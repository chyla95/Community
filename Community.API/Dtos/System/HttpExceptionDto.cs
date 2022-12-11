namespace Community.API.Dtos.System
{
#pragma warning disable CS8618
    public class HttpExceptionMessageDto
    {
        public string Exception { get; set; }
    }

    public class HttpExceptionDto
    {
        public IEnumerable<HttpExceptionMessageDto> Exceptions { get; set; }
    }
#pragma warning restore CS8618
}
