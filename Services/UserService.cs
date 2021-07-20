using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using keepnotes_api.DTOs;
using keepnotes_api.Helpers;
using keepnotes_api.Interfaces;
using keepnotes_api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using BCryptNet = BCrypt.Net.BCrypt;

namespace keepnotes_api.Services
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
        public async Task<IEnumerable<User>> Get() => await _user.Find(users => true).ToListAsync();

        // Get User by Id
        public async Task<User> Get(string userId) => await _user.Find(user => user.Id == userId).FirstOrDefaultAsync();
        
        // Update User
        public async Task<bool> Update(string userId, User user)
        {
            var hashPassword = BCryptNet.HashPassword(user.Password);
    
            var filter = Builders<User>.Filter.Eq(x => x.Id, userId);
            var update = Builders<User>.Update
                .Set(x => x.Username, user.Username)
                .Set(x => x.Email, user.Email)
                .Set(x => x.Password, hashPassword)
                .Set(x => x.ProfileImageUrl, user.ProfileImageUrl);

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