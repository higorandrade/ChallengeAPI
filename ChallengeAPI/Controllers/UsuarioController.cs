using ChallengeAPI.DTOs;
using ChallengeAPI.Models;
using ChallengeAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using System.Threading.Tasks;

namespace ChallengeAPI.Controllers 
{
    [Controller]
    [Authorize(Roles = "admin")]
    [Route("challenge-api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IRepository repository;
        public UsuarioController(IRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Adiciona um usuário ao banco de dados.
        /// </summary>
        /// <remarks>
        /// Exemplo:
        ///
        ///     {
        ///         "username": "login",
        ///         "password": "password",
        ///         "role": "admin" 
        ///      }
        ///
        /// </remarks>
        /// <response code="201">Sucesso.</response>
        /// <response code="400">Formato de entrada inválido.</response>
        /// <response code="901">Login já cadastrado.</response>
        [HttpPost]
        [AllowAnonymous]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult> Post([FromBody] Usuario usuario)
        {
            if (usuario == null)
            {
                return BadRequest("Formato de entrada inválido.");
            }

            bool usuarioCadastrado = (await repository.UsuarioCadastradoAsync(usuario.Username));
            if (usuarioCadastrado)
            {
                return StatusCode(901, "Login já cadastrado.");
            }
            UsuarioDTO novoUsuario = new UsuarioDTO(await repository.InsertUsuarioAsync(usuario));
            return Created("", novoUsuario);
        }

        /// <summary>
        /// Autentica usuário gerando um token.
        /// </summary>
        /// <response code="200">Sucesso.</response>
        /// <response code="401">Usuário não autenticado.</response>
        [HttpGet]
        [Route("login")]
        [AllowAnonymous]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> Login(string username, string password)
        {
            Usuario usuario = new Usuario(username, password);
            UsuarioComToken usuarioComToken = await repository.LoginAsync(usuario);
            if (usuarioComToken != null)
            {
                return Ok(usuarioComToken);
            }
            return Unauthorized("Usuário não autenticado.");
        }

        /// <summary>
        /// Altera um usuário do banco de dados.
        /// (Deve estar autenticado como "admin")
        /// </summary>
        /// <remarks>
        /// Exemplo:
        ///
        ///     {
        ///         "username": "login",
        ///         "password": "password",
        ///         "role": "admin" 
        ///      }
        ///
        /// </remarks>
        /// <response code="200">Sucesso.</response>
        /// <response code="400">Formato de entrada inválido.</response>
        /// <response code="403">Não autorizado.</response>
        /// <response code="404">Username não encontrado.</response>
        [HttpPut("{username}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult> Put([FromRoute] string username, [FromBody] Usuario usuario)
        {
            if (usuario == null)
            {
                return BadRequest("Formato de entrada inválido.");
            }

            Usuario usuarioAtualizado = (await repository.UpdateUsuarioAsync(username, usuario));
            if (usuarioAtualizado == null)
            {
                return NotFound();
            }
            UsuarioDTO novoUsuario = new UsuarioDTO(usuarioAtualizado);
            return Ok(novoUsuario);
        }

        /// <summary>
        /// Exclui um usuário do banco de dados.
        /// (Deve estar autenticado como "admin")
        /// </summary>
        /// <response code="204">Sucesso.</response>
        /// <response code="403">Não autorizado.</response>
        [HttpDelete("{username}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteAsync([FromRoute] string username)
        {
            await repository.DeleteUsuarioAsync(username);
            return NoContent();
        }

    }
}
