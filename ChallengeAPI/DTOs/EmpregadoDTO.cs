using ChallengeAPI.Models;
using System.Collections.Generic;

namespace ChallengeAPI.DTOs
{
    public class EmpregadoDTO
    {
        public EmpregadoDTO()
        {
            this.Projetos = new List<int>();
        }
        public EmpregadoDTO(Empregado empregado)
        {
            this.Id_empregado = empregado.Id_empregado;
            this.Primeiro_nome = empregado.Primeiro_nome;
            this.Ultimo_nome = empregado.Ultimo_nome;
            this.Telefone = empregado.Telefone;
            this.Endereco = empregado.Endereco;
            this.Projetos = new List<int>();
            foreach (var i in empregado.Projetos)
            {
                this.Projetos.Add(i.Id_projeto);
            }
        }
        public int Id_empregado { get; set; }
        public string Primeiro_nome { get; set; }
        public string Ultimo_nome { get; set; }
        public string Telefone { get; set; }
        public string Endereco { get; set; }
        public List<int> Projetos { get; set; }
    }
}
