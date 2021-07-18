using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using keepnotes_api.DTOs;
using keepnotes_api.Interfaces;
using keepnotes_api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;

namespace keepnotes_api.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> _user;

        

        public UserService(IKeepNotesDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _user = database.GetCollection<User>(settings.UsersCollectionName);

            
        }

        // Get All Users
        public List<User> GetUsers()
        {
            var userList = _user.Find(user => true).ToList();

            return userList;
        }

        // Get User by Id
        public User GetUser(string id) => _user.Find<User>(user => user.Id == id).FirstOrDefault();
        
        // Update User
        /*public void UpdateUser(string id, User updatedUser) => _user.ReplaceOne(user => user.Id == id, updatedUser);*/
        
        // Delete User
        public void DeleteUser(string id) => _user.DeleteOne(user => user.Id == id);
    }
}