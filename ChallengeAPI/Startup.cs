using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using ChallengeAPI.Data;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Reflection;
using ChallengeAPI.Repositories;
using ChallengeAPI.Services;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace ChallengeAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string sqlConnectionString = Configuration.GetConnectionString("DefaultConnection");

            services.AddDbContextPool<DataContext>(Options => Options.UseMySql(sqlConnectionString, ServerVersion.AutoDetect(sqlConnectionString)));

            services.AddScoped<IRepository, Repository>();

            services.AddControllers();
            services.AddRouting(options => options.LowercaseUrls = true);

            services.AddSingleton<IJWTService, JWTService>();

            var key = Encoding.ASCII.GetBytes(Configuration.GetSection("JWT:Secret").Value);

            services.AddAuthentication(p =>
            {
                p.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                p.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(p =>
            {
                p.RequireHttpsMetadata = false;
                p.SaveToken = true;
                p.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddSwaggerGen(c =>
            {
            c.SwaggerDoc("v1", 
                new OpenApiInfo
                {
                    Title = "Challenge API",
                    Version = "v1",
                    Description = "WebAPI desenvolvida em .NET utilizando o EntityFramework, Swagger e JWT.\n"+
                    "\nInstru��es de uso:\n Para utilizar esta WebAPI pela primeira vez, voc� deve realizar " +
                    "o cadastro de um novo usu�rio\n na base de dados.Utilize o endpoint de POST de Usuario para isso." +
                    "Voc� deve se cadastrar como\n 'admin', 'gerente' ou 'projetista'.\n\n"+
                    "- Autentica��o\n\nObtenha um token inserindo username e senha no endpoint de login.\n\n" +
                    "- Autoriza��o\n\nClique no bot�o de autoriza��o (Authorize) e insira 'Bearer' + (espa�o) + token.\n\n" +
                    "- Permiss�es\n\n1. Os endpoints de cadastro de Usu�rio e login n�o requerem autoriza��o.\n"+
                    "2. Para utilizar o endpoint de cadastro de empregado voc� deve ter fun��o (role) de 'gerente'.\n"+
                    "3. Para utilizar o endpoint de cadastro de projeto voc� deve ter fun��o (role) de 'gerente' ou 'projetista'.\n"+
                    "4. Para utilizar os endpoints de dele��o e altera��o de usu�rio voc� deve ter fun��o (role) de 'admin'.\n"+
                    "5. Todos os endpoints podem ser acessados pelo 'admin'",
                    Contact = new OpenApiContact
                    {
                        Name = "Higor Andrade",
                        Email = "higor_andrade@outlook.com",
                        Url = new System.Uri("https://github.com/higorandrade")
                    }
                });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Insira: 'Bearear ' + token",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                    new string[]{ }
                }
            });

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
                
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ChallengeAPI v1");
                    c.DefaultModelsExpandDepth(-1);
                });
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
