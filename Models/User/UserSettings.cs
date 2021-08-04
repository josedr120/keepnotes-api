using MongoDB.Bson.Serialization.Attributes;

namespace keepnotes_api.Models.User
{
    public class UserSettings : IUserSettings
    {
        [BsonElement("theme")]
        public string Theme { get; set; }
        
        [BsonElement("language")]
        public string Language { get; set; }
    }
}