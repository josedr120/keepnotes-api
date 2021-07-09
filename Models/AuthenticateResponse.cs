using System.Data.Common;

namespace keepnotes_api.Models
{
    public class AuthenticatedResponse
    {
        // ReSharper disable once MemberCanBePrivate.Global
        public string Id { get; set; }
        // ReSharper disable once MemberCanBePrivate.Global
        public string Username { get; set; }
        // ReSharper disable once MemberCanBePrivate.Global
        public string Token { get; set; }

        public AuthenticatedResponse(User user, string token)
        {
            Id = user.Id;
            Username = user.Username;
            Token = token;
        }
    }
}