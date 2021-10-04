using ChallengeAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChallengeAPI.DTOs
{
    public class UsuarioComToken
    {
        public UsuarioComToken()
        {

        }
        public UsuarioComToken(Usuario usuario, string token)
        {
            this.Username = usuario.Username;
            this.Role = usuario.Role;
            this.Token = token;
        }
        public string Username { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
    }
}
