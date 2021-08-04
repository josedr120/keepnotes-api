using System.Collections.Generic;
using System.Threading.Tasks;
using keepnotes_api.DTOs;
using keepnotes_api.Models.User;

namespace keepnotes_api.Services.UserService
{
    public interface IUserService
    {
        Task<UserDto> Get(string userId);

        Task<List<UserDto>> Get();

        Task<bool> Update(string userId, User user);

        Task<bool> ResetPassword(string userId, User user);

        Task<bool> Delete(string userId);
    }
}