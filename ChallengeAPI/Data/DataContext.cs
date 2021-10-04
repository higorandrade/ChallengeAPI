using ChallengeAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ChallengeAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext (DbContextOptions<DataContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Empregado>()
                .HasMany(e => e.ProjetosGerenciados)
                .WithOne(p => p.Gerente)
                .HasForeignKey(x => x.Id_gerente);

            modelBuilder.Entity<Projeto>()
                .HasMany(p => p.Empregados)
                .WithMany(e => e.Projetos)
                .UsingEntity(x => x.ToTable("Membros"));
        }

        public DbSet<Empregado> Empregados { get; set; }
        public DbSet<Projeto> Projetos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
    }
}