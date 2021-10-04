using ChallengeAPI.DTOs;
using System.ComponentModel.DataAnnotations;

namespace ChallengeAPI.Models
{
    public class Usuario
    {
        public Usuario()
        {

        }
        public Usuario(string username, string password)
        {
            this.Username = username;
            this.Password = password;
        }
        [Key]
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
