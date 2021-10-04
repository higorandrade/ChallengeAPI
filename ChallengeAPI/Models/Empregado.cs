using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using ChallengeAPI.DTOs;

namespace ChallengeAPI.Models
{
    public class Empregado
    {
        public Empregado()
        {
            this.Projetos = new HashSet<Projeto>();
            this.ProjetosGerenciados = new HashSet<Projeto>();
        }
        public Empregado(EmpregadoDTO empregadoDTO)
        {
            this.Id_empregado = empregadoDTO.Id_empregado;
            this.Primeiro_nome = empregadoDTO.Primeiro_nome;
            this.Ultimo_nome = empregadoDTO.Ultimo_nome;
            this.Telefone = empregadoDTO.Telefone;
            this.Endereco = empregadoDTO.Endereco;
            this.Projetos = new HashSet<Projeto>();
            this.ProjetosGerenciados = new HashSet<Projeto>();
        }
        [Key]
        public int Id_empregado { get; set; }
        [StringLength(255)]
        public string Primeiro_nome { get; set; }
        [StringLength(255)]
        public string Ultimo_nome { get; set; }
        [StringLength(10)]
        public string Telefone { get; set; }
        public string Endereco { get; set; }
        public ICollection<Projeto> ProjetosGerenciados { get; set; }
        public ICollection<Projeto> Projetos { get; set; }
    }
}