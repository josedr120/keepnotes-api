using keepnotes_api.Interfaces;

namespace keepnotes_api.DTOs
{
    public class UserDto
    {
        public string Id { get; set; }
        
        public string Username { get; set; }

        public string Email { get; set; }
        
        public string ProfileUrl { get; set; }
    }
}