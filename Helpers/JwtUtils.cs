using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security;
using System.Security.Authentication;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using keepnotes_api.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using static System.DateTime;

namespace keepnotes_api.Helpers
{
    public interface IJwtUtils
    {
        string GenerateJwtToken(IUser user);

        bool VerifyJwtToken(string token);

        void RevokeToken();

        RefreshToken GenerateRefreshToken(string ipAddress);
    }
    
    
    public class JwtUtils : IJwtUtils
    {

        private readonly string _jwtSecretKey;
        
          public JwtUtils(IConfiguration configuration)
        {
            _jwtSecretKey = configuration.GetSection("JwtSecretKey").ToString();
        }

          
          public string GenerateJwtToken(IUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenKey = Encoding.ASCII.GetBytes(_jwtSecretKey);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new("Id", user.Id)
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

        public bool VerifyJwtToken(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return false;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSecretKey);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out var validatedToken);

                // return user id from JWT token if validation successful
                return true;
            }
            catch
            {
                // return null if validation fails
                return false;
            }
        }

        public void RevokeToken()
        {
            throw new System.NotImplementedException();
        }

        public RefreshToken GenerateRefreshToken(string ipAddress)
        {
            var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[64];
            
            rngCryptoServiceProvider.GetBytes(randomBytes);
            var refreshToken = new RefreshToken()
            {
                Token = Convert.ToBase64String(randomBytes),
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.Now,
                CreatedByIp = ipAddress
            };
            
            return refreshToken;
        }
    }
}