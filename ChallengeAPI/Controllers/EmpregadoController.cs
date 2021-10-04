using ChallengeAPI.DTOs;
using ChallengeAPI.Repositories;
using ChallengeAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace ChallengeAPI.Controllers
{
    [Controller]
    [Authorize(Roles ="gerente, admin")]
    [Route("challenge-api/[controller]")]
    public class EmpregadoController : ControllerBase
    {
        private readonly IRepository repository;

        public EmpregadoController(IRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Adiciona um empregado ao banco de dados.
        /// (Deve estar autenticado como "admin" ou "gerente")
        /// </summary>
        /// <remarks>
        /// Exemplo:
        ///
        ///     {
        ///         "id_empregado": 1,
        ///         "primeiro_nome": "Jose",
        ///         "ultimo_nome": "Silva",
        ///         "telefone": "0123456789",
        ///         "endereco": "Brasil",
        ///         "projetos": [ 1, 2, 3 ]
        ///     }
        ///     
        /// Observações:
        /// 
        /// Caso oculto, id é gerado automaticamente.
        /// Todos os campos são opcionais, mas, caso informado, telefone deve ser uma string numérica de 10 dígitos.
        /// Ids de projetos inexistentes são ignorados.
        ///
        /// </remarks>
        /// <response code="201">Sucesso.</response>
        /// <response code="400">Formato de entrada inválido</response>
        /// <response code="401">Não autorizado.</response>
        /// <response code="900">Id já cadastrado.</response>
        /// <response code="010">Formato do campo telefone inválido.</response>
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult> PostEmpregado([FromBody] EmpregadoDTO empregadoDTO)
        {
            if(empregadoDTO == null)
            {
                return BadRequest("Formato de entrada inválido.");
            }

            bool empregadoCadastrado = (await repository.EmpregadoCadastradoAsync(empregadoDTO.Id_empregado));
            if (empregadoCadastrado)
            {
                return StatusCode(900, "Id já cadastrado.");
            }

            // Validacao do campo de Telefone
            if (empregadoDTO.Telefone != null)
            {
                if (empregadoDTO.Telefone.Length != 10)
                {
                    return StatusCode(010, "Telefone deve conter 10 dígitos.");
                }
                if (!Int64.TryParse(empregadoDTO.Telefone, out long number))
                {
                    return StatusCode(010, "Telefone deve ser numérico.");
                }
            }

            Empregado empregado = new Empregado(empregadoDTO);

            if (empregadoDTO.Projetos != null)
            {
                foreach (int i in empregadoDTO.Projetos)
                {
                    Projeto p = (await repository.GetProjetoAsync(i));
                    if (p != null)
                    {
                        empregado.Projetos.Add(p);
                        p.Empregados.Add(empregado);
                    }
                }
            }
            
            Empregado novoEmpregado = (await repository.InsertEmpregadoAsync(empregado));
            EmpregadoDTO novoEmpregadoDTO = new EmpregadoDTO(novoEmpregado);
            
            return Created("", novoEmpregadoDTO);
        }
    }
}
