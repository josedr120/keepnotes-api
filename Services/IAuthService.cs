using System.Threading.Tasks;
using keepnotes_api.Models;

namespace keepnotes_api.Services
{
    public interface IAuthService
    {
        Task<AuthenticatedResponse> Register(User user);

        Task<AuthenticatedResponse> Login(Login login);
    }
}