
using ChallengeAPI.Models;

namespace ChallengeAPI.Services
{
    public interface IJWTService
    {
        string GenerateTokens(Usuario usuario);
    }
}
