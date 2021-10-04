using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using ChallengeAPI.DTOs;

namespace ChallengeAPI.Models
{
    public class Projeto
    {
        public Projeto()
        {
            this.Empregados = new HashSet<Empregado>();
        }
        public Projeto(ProjetoDTO projetoDTO)
        {
            this.Id_projeto = projetoDTO.Id_projeto;
            this.Nome = projetoDTO.Nome;
            this.Data_criacao = projetoDTO.Data_criacao;
            this.Data_termino = projetoDTO.Data_termino;
            this.Id_gerente = projetoDTO.Id_gerente;
            this.Empregados = new HashSet<Empregado>();
        }
        [Key]
        public int Id_projeto { get; set; }
        [StringLength(255)]
        public string Nome { get; set; }
        public DateTime Data_criacao { get; set; }
        public DateTime? Data_termino { get; set; }
        [Column("Gerente")]
        public int Id_gerente { get; set; }
        [ForeignKey("Id_gerente")]
        public Empregado Gerente { get; set; }
        public ICollection<Empregado> Empregados { get; set; }
    }
}