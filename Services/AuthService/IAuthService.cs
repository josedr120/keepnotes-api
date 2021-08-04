using System.Threading.Tasks;
using keepnotes_api.Models;
using keepnotes_api.Models.Auth;
using keepnotes_api.Models.User;

namespace keepnotes_api.Services.AuthService
{
    public interface IAuthService
    {
        Task<AuthenticatedResponse> Register(User user);

        Task<AuthenticatedResponse> Login(Login login);
    }
}