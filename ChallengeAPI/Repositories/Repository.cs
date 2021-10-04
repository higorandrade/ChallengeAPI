using ChallengeAPI.Data;
using ChallengeAPI.DTOs;
using ChallengeAPI.Models;
using ChallengeAPI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace ChallengeAPI.Repositories
{
    public class Repository : IRepository
    {
        private readonly DataContext context;
        private readonly IJWTService jwt;
        public Repository(DataContext context, IJWTService jwt)
        {
            this.context = context;
            this.jwt = jwt;
        }

        public async Task<bool> ProjetoCadastradoAsync(int id)
        {
            var projeto = (await context.Projetos.FindAsync(id));
            return (projeto != null);
        }
        public async Task<bool> EmpregadoCadastradoAsync(int id)
        {
            var empregado = (await context.Empregados.FindAsync(id));
            return (empregado != null);
        }
        public async Task<bool> UsuarioCadastradoAsync(string username)
        {
            var usuario = (await context.Usuarios.FindAsync(username));
            return (usuario != null);
        }
        public async Task<Empregado> GetEmpregadoAsync(int id)
        {
            return await context.Empregados.FindAsync(id);
        }
        public async Task<Projeto> GetProjetoAsync(int id)
        {
            return await context.Projetos.FindAsync(id);
        }
        public async Task<Usuario> GetUsuarioAsync(string username)
        {
            return await context.Usuarios.FindAsync(username);
        }
        public async Task<Empregado> InsertEmpregadoAsync(Empregado empregado)
        {
            await context.Empregados.AddAsync(empregado);
            await context.SaveChangesAsync();

            return empregado;
        }
        public async Task<Projeto> InsertProjetoAsync(Projeto projeto)
        {
            await context.Projetos.AddAsync(projeto);
            await context.SaveChangesAsync();

            return projeto;
        }
        public async Task<Usuario> InsertUsuarioAsync(Usuario usuario)
        {
            PasswordToHash(usuario);
            await context.Usuarios.AddAsync(usuario);
            await context.SaveChangesAsync();

            return usuario;
        }
        public async Task<Usuario> UpdateUsuarioAsync(string username, Usuario usuario)
        {
            PasswordToHash(usuario);
            var usuarioConsultado = await context.Usuarios.FindAsync(username);
            if (usuarioConsultado == null)
            {
                return null;
            }
            context.Entry(usuarioConsultado).CurrentValues.SetValues(usuario);
            await context.SaveChangesAsync();
            return usuarioConsultado;
        }
        public async Task<Usuario> DeleteUsuarioAsync(string username)
        {
            Usuario usuarioConsultado = (await context.Usuarios.FindAsync(username));
            if (usuarioConsultado == null)
            {
                return null;
            }
            var usuarioRemovido = context.Usuarios.Remove(usuarioConsultado);
            await context.SaveChangesAsync();
            return usuarioRemovido.Entity;
        }
        public async Task<UsuarioComToken> LoginAsync(Usuario usuario)
        {
            Usuario usuarioConsultado = (await context.Usuarios.FindAsync(usuario.Username));
            if (usuarioConsultado == null)
            {
                return null;
            }

            var passwordHasher = new PasswordHasher<Usuario>();
            var status = passwordHasher.VerifyHashedPassword(usuario, usuarioConsultado.Password, usuario.Password);
            switch(status)
            {
                case PasswordVerificationResult.Failed:
                    return null;
                case PasswordVerificationResult.Success:
                    string token = jwt.GenerateTokens(usuarioConsultado);
                    UsuarioComToken usuarioComToken = new UsuarioComToken(usuarioConsultado, token);
                    return usuarioComToken;
                case PasswordVerificationResult.SuccessRehashNeeded:
                    await UpdateUsuarioAsync(usuario.Username, usuario);
                    token = jwt.GenerateTokens(usuarioConsultado);
                    usuarioComToken = new UsuarioComToken(usuarioConsultado, token);
                    return usuarioComToken;
                default:
                    throw new InvalidOperationException();
            }
        }
        private void PasswordToHash(Usuario usuario)
        {
            var passwordHasher = new PasswordHasher<Usuario>();
            usuario.Password = passwordHasher.HashPassword(usuario, usuario.Password);
        }
    }
}