using System.Collections.Generic;
using System.Threading.Tasks;
using keepnotes_api.Models;
using MongoDB.Driver;

namespace keepnotes_api.Services
{
    public interface IUserService
    {
        Task<User> Get(string userId);

        Task<IEnumerable<User>> Get();

        Task<bool> Update(string userId, User user);

        Task<bool> Delete(string userId);
    }
}