using System.Collections.Generic;

namespace keepnotes_api.Models.User
{
    public interface IUser
    {
        string Id { get; set; }

        string Username { get; set; }

        string Email { get; set; }

        string ProfileImageUrl { get; set; }
        
        List<UserSettings> Settings { get; set; }
    }
}