using ChallengeAPI.Models;
using System;
using System.Collections.Generic;

namespace ChallengeAPI.DTOs
{
    public class ProjetoDTO
    {
        public ProjetoDTO()
        {
            this.Empregados = new List<int>();
        }
        public ProjetoDTO(Projeto projeto)
        {
            this.Id_projeto = projeto.Id_projeto;
            this.Nome = projeto.Nome;
            this.Data_criacao = projeto.Data_criacao;
            this.Data_termino = projeto.Data_termino;
            this.Id_gerente = projeto.Id_gerente;
            this.Empregados = new List<int>();
            foreach (var i in projeto.Empregados)
            {
                this.Empregados.Add(i.Id_empregado);
            }
        }
        public int Id_projeto { get; set; }
        public string Nome { get; set; }
        public DateTime Data_criacao { get; set; }
        public DateTime? Data_termino { get; set; }
        public int Id_gerente { get; set; }
        public List<int> Empregados { get; set; }
    }
}