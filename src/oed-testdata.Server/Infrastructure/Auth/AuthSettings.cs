namespace oed_testdata.Server.Infrastructure.Auth
{
    public class AuthSettings
    {
        public required string Username { get; set; }
        public required string Password { get; set; }

        public required string CloudEventQueryParamName { get; set; }
        public required string CloudEventSecret { get; set; }
    }
}
