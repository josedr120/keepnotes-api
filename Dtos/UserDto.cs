using System.Collections.Generic;
using keepnotes_api.Interfaces;
using keepnotes_api.Models.User;

namespace keepnotes_api.DTOs
{
    public class UserDto
    {
        public string Id { get; set; }
        
        public string Username { get; set; }

        public string Email { get; set; }
        
        public string ProfileUrl { get; set; }
        
        public List<UserSettings> Settings { get; set; }
    }
}