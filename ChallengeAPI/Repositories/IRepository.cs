using ChallengeAPI.DTOs;
using ChallengeAPI.Models;
using System.Threading.Tasks;

namespace ChallengeAPI.Repositories
{
    public interface IRepository
    {
        Task<bool> ProjetoCadastradoAsync(int id);
        Task<bool> EmpregadoCadastradoAsync(int id);
        Task<bool> UsuarioCadastradoAsync(string username);
        Task<Empregado> GetEmpregadoAsync(int id);
        Task<Projeto> GetProjetoAsync(int id);
        Task<Usuario> GetUsuarioAsync(string username);
        Task<Empregado> InsertEmpregadoAsync(Empregado empregado);
        Task<Projeto> InsertProjetoAsync(Projeto projeto);
        Task<Usuario> InsertUsuarioAsync(Usuario usuario);
        Task<Usuario> UpdateUsuarioAsync(string username, Usuario usuario);
        Task<Usuario> DeleteUsuarioAsync(string username);
        Task<UsuarioComToken> LoginAsync(Usuario usuario);
        //Task<ICollection<Usuario>> getUsuariosAsync();
    }
}
