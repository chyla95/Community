namespace Community.API.Utilities
{
    public static class Constants
    {
        public static class AppSettingsKeys
        {
            public const string DB_CONNECTION_STRING = "ConnectionStrings:DatabaseConnection";
            public const string JWT_SECRET = "AppSettings:JwtSecret";
        }

        public static class HttpContextItemKeys
        {
            public const string USER = "User";
        }
    }
}
