using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using keepnotes_api.Interfaces;
using keepnotes_api.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using static System.DateTime;
using BCryptNet = BCrypt.Net.BCrypt;

namespace keepnotes_api.Services
{
    public class AuthService
    {
        private readonly IMongoCollection<User> _user;

        private readonly string _jwtSecretKey;


        public AuthService(IKeepNotesDatabaseSettings settings, IConfiguration configuration)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _user = database.GetCollection<User>(settings.UsersCollectionName);

            _jwtSecretKey = configuration.GetSection("JwtSecretKey").ToString();
        }

        public AuthenticatedResponse Register(User user)
        {
            var salt = BCryptNet.GenerateSalt(10);
            var hasPassword = BCryptNet.HashPassword(user.Password, salt);

            user.Password = hasPassword;
            _user.InsertOne(user);

            var token = GenerateJwtToken(user);

            return new AuthenticatedResponse(user, token);
        }


        public AuthenticatedResponse Login(Login login)
        {
            var user = _user.Find(x => x.Username == login.Username && x.Password == login.Password).FirstOrDefault();

            if (user == null)
            {
                return null;
            }

            var token = GenerateJwtToken(user);

            return new AuthenticatedResponse(user, token);
        }


        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenKey = Encoding.ASCII.GetBytes(_jwtSecretKey);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("Id", user.Id)
                }),

                Expires = UtcNow.AddHours(1),

                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}