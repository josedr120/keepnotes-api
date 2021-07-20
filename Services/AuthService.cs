using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using keepnotes_api.Helpers;
using keepnotes_api.Interfaces;
using keepnotes_api.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using static System.DateTime;
using BCryptNet = BCrypt.Net.BCrypt;

namespace keepnotes_api.Services
{
    public class AuthService : IAuthService
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

        public async Task<AuthenticatedResponse> Register(User user)
        {

            var salt = BCryptNet.GenerateSalt(10);
            var hashPassword = BCryptNet.HashPassword(user.Password, salt);

            user.ProfileImageUrl =
                $"https://ui-avatars.com/api/?name={user.Username}&background={RandomColor()}&color=fff";
            user.Password = hashPassword;
            await _user.InsertOneAsync(user);

            var token = GenerateJwtToken(user);

            return new AuthenticatedResponse(user, token);
        }


        public async Task<AuthenticatedResponse> Login(Login login)
        {
            var filter = Builders<User>.Filter.Eq(x => x.Username, login.Username);
            var user = await _user.Find(filter).FirstOrDefaultAsync();

            if (user == null)
            {
                return null;
            }

            var hashVerify = BCryptNet.Verify(login.Password, user.Password);

            if (!hashVerify)
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
                    new Claim("Id", user.Id.ToString())
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
        
        /*const randomColor = (): string => {
            let randomColor = '';
            let char = '0123456789abcdefghijklmnopqrstuvwxyz';

            for (let i = 0; i < 6; i++) {
                randomColor = randomColor + char[Math.floor(Math.random() * 16)];
            }

            return randomColor;
        };*/

        private string RandomColor()
        {
            var rand = new Random();
            var randomColor = "";
            var Char = "0123456789abcdefghijklmnopqrstuvwxyz";

            for (int i = 0; i < 6; i++)
            {
                randomColor += Char[(int) Math.Floor((double) rand.Next(16))];
            }

            return randomColor;
        }
    }
}