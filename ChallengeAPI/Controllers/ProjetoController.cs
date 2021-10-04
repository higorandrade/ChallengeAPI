using ChallengeAPI.DTOs;
using ChallengeAPI.Repositories;
using ChallengeAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace ChallengeAPI.Controllers
{
    [Controller]
    [Authorize(Roles = "gerente, admin, projetista")]
    [Route("challenge-api/[controller]")]
    public class ProjetoController : ControllerBase
    {
        private readonly IRepository repository;

        public ProjetoController(IRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Adiciona um projeto ao banco de dados.
        /// (Deve estar autenticado como "admin", "gerente" ou "projetista")
        /// </summary>
        /// <remarks>
        /// Exemplo:
        ///
        ///     {
        ///         "id_projeto": 1,
        ///         "nome": "web-api",
        ///         "data_criacao": "0001-01-01T00:00:00.000Z",
        ///         "data_termino": "0001-01-01T00:00:00.000Z",
        ///         "id_gerente": 1,
        ///         "empregados": [ 2, 3 ]
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Sucesso.</response>
        /// <response code="400">Formato de entrada inválido.</response>
        /// <response code="900">Id já cadastrado.</response>
        /// <response code="404">Gerente do projeto não encontrado.</response>
        /// <response code="403">Não autorizado.</response>
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult> PostProjeto([FromBody] ProjetoDTO projetoDTO)
        {
            if (projetoDTO == null)
            {
                return BadRequest("Formato de entrada inválido.");
            }

            bool projetoCadastrado = (await repository.ProjetoCadastradoAsync(projetoDTO.Id_projeto));
            if (projetoCadastrado)
            {
                return StatusCode(900, "Id já cadastrado.");
            }

            bool gerenteCadastrado = (await repository.EmpregadoCadastradoAsync(projetoDTO.Id_gerente));
            if(!gerenteCadastrado)
            {
                return NotFound("Gerente do projeto não encontrado.");
            }
            
            Projeto projeto = new Projeto(projetoDTO);
            
            if(projetoDTO.Empregados != null)
            {
                foreach (int i in projetoDTO.Empregados)
                {
                    Empregado e = (await repository.GetEmpregadoAsync(i));
                    if (e != null)
                    {
                        projeto.Empregados.Add(e);
                        e.Projetos.Add(projeto);
                    }
                }
            }

            projeto.Gerente = (await repository.GetEmpregadoAsync(projeto.Id_gerente));
            projeto.Gerente.ProjetosGerenciados.Add(projeto);

            // cadastra Gerente como membro do projeto
            projeto.Empregados.Add(projeto.Gerente);
            projeto.Gerente.Projetos.Add(projeto);

            Projeto novoProjeto = (await repository.InsertProjetoAsync(projeto));
            ProjetoDTO novoProjetoDTO = new ProjetoDTO(novoProjeto);

            return Created("", novoProjetoDTO);
        }
    }
}