namespace keepnotes_api.Models.Auth
{
    public interface ILogin
    {
        string Username { get; set; }

        string Password { get; set; }
    }
}