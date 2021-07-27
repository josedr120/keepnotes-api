namespace keepnotes_api.Helpers
{
    public class AppSettings : IAppSettings
    {
        public string JwtSecretKey { get; set; }
    }

    internal interface IAppSettings
    {
        string JwtSecretKey { get; set; }
    }
}