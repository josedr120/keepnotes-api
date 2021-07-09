using System.ComponentModel.DataAnnotations;
using keepnotes_api.Interfaces;

namespace keepnotes_api.Models
{
    public class Login : ILogin
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}