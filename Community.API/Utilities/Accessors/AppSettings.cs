namespace Community.API.Utilities.Accessors
{
    public class AppSettings : IAppSettings
    {
        private readonly IConfiguration _configuration;

        public AppSettings(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetValue(string key)
        {
            if (string.IsNullOrEmpty(key)) throw new NullReferenceException(nameof(key));

            string? value = _configuration.GetSection(key).Value;
            if (string.IsNullOrEmpty(key)) throw new NullReferenceException(nameof(value));

            return value!;
        }
    }
}
