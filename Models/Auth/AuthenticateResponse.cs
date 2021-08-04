using System.Data.Common;
using keepnotes_api.Models.User;

namespace keepnotes_api.Models.Auth
{
    public class AuthenticatedResponse
    {
        // ReSharper disable once MemberCanBePrivate.Global
        public string Id { get; set; }
        // ReSharper disable once MemberCanBePrivate.Global
        public string Username { get; set; }
        // ReSharper disable once MemberCanBePrivate.Global
        public string Token { get; set; }

        public AuthenticatedResponse(IUser user, string token)
        {
            Id = user.Id;
            Username = user.Username;
            Token = token;
        }
    }
}