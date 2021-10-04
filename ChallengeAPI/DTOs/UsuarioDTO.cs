using ChallengeAPI.Models;

namespace ChallengeAPI.DTOs
{
    public class UsuarioDTO
    {
        public UsuarioDTO(Usuario usuario)
        {
            this.Username = usuario.Username;
            this.Role = usuario.Role;
        }
        public string Username { get; set; }
        public string Role { get; set; }
    }
}