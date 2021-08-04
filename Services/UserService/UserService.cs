using System.Collections.Generic;
using System.Threading.Tasks;
using keepnotes_api.DTOs;
using keepnotes_api.Models.Db;
using keepnotes_api.Models.Note;
using keepnotes_api.Models.User;
using MongoDB.Driver;
using BCryptNet = BCrypt.Net.BCrypt;

namespace keepnotes_api.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IMongoCollection<User> _user;
        private readonly IMongoCollection<Note> _note;

        public UserService(IKeepNotesDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _user = database.GetCollection<User>(settings.UsersCollectionName);
            _note = database.GetCollection<Note>(settings.NotesCollectionName);
        }

        // Get All Users
        public async Task<List<UserDto>> Get()
        {
            var e = await _user.Find(x => true).ToListAsync();
            var users = new UserDto();
            
            e.ForEach(x =>
            {
                users.Id = x.Id;
                users.Username = x.Username;
                users.Email = x.Email;
                users.ProfileUrl = x.ProfileImageUrl;
                users.Settings = x.Settings;
            });

            return new List<UserDto>() {users};
        }

        // Get User by Id
        public async Task<UserDto> Get(string userId)
        {
            var e = await _user.Find(user => user.Id == userId).FirstOrDefaultAsync();

            var userDto = new UserDto()
            {
                Id = e.Id,
                Username = e.Username,
                Email = e.Email,
                ProfileUrl = e.ProfileImageUrl,
                Settings = e.Settings
            };
            
            return userDto;
        }

        // Update User
        public async Task<bool> Update(string userId, User user)
        {
            var hashPassword = BCryptNet.HashPassword(user.Password);
    
            var filter = Builders<User>.Filter.Eq(x => x.Id, userId);
            var update = Builders<User>.Update
                .Set(x => x.Username, user.Username)
                .Set(x => x.Email, user.Email)
                .Set(x => x.ProfileImageUrl, user.ProfileImageUrl)
                .Set(x => x.Settings, user.Settings);

            var result = await _user.UpdateOneAsync(filter, update);
            
            return result.ModifiedCount == 1;
        }
        
        // Reset Password or Change Password
        public async Task<bool> ResetPassword(string userId, User user)
        {
            var hashPassword = BCryptNet.HashPassword(user.Password);
    
            var filter = Builders<User>.Filter.Eq(x => x.Id, userId);
            var update = Builders<User>.Update
                .Set(x => x.Password, hashPassword);
            

            var result = await _user.UpdateOneAsync(filter, update);
            
            return result.ModifiedCount == 1;
        }

        // Delete User
        public async Task<bool> Delete(string userId)
        {
            var filterUser = Builders<User>.Filter.Eq(x => x.Id, userId);
            
            var filterUserNotes = Builders<Note>.Filter.Eq(x => x.UserId, userId);
            await _note.DeleteManyAsync(filterUserNotes);
            
            var result = await _user.DeleteOneAsync(filterUser);

            return result.DeletedCount == 1;
        }
    }
}