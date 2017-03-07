namespace ModularSystem.Common.Wpf.Context
{
    public class AuthenticationContext
    {
        public string ClientId { get; set; }
        public string ClientPassword { get; set; } // TODO: store somewhere invivisble
        public string UserName { get; set; }

        public string AccessToken { get; set; }
    }
}
