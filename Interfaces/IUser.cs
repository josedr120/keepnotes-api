namespace keepnotes_api.Interfaces
{
    public interface IUser
    {

        string Username { get; set; }

        string Email { get; set; }
        
        string ProfileUrl { get; set; }
    }
}