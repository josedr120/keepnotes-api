namespace keepnotes_api.Models
{
    public interface IUser
    {
        string Id { get; set; }

        string Username { get; set; }

        string Email { get; set; }

        string ProfileImageUrl { get; set; }
    }
}