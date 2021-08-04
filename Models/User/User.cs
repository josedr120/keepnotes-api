using System.Collections.Generic;
using System.Text.Json.Serialization;
using keepnotes_api.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace keepnotes_api.Models.User
{
    public class User: IUser
    {
        [BsonId]
        [BsonIgnoreIfDefault]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        [BsonElement("username")]
        public string Username { get; set; }
        
        [BsonElement("email")]
        public string Email { get; set; }

        [BsonElement("password")]
        public string Password { get; set; }
        
        [BsonElement("profileImageUrl")]
        public string ProfileImageUrl { get; set; }

        [BsonElement("settings")]
        public List<UserSettings> Settings { get; set; }
    }
}