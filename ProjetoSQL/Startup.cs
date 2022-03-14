using ProjetoSQL.Data;
using ProjetoSQL.Repository;
using ProjetoSQL.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Text;
using System.Text.Json.Serialization;
using ProjetoSQL.Models;

namespace ProjetoSQL
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
            var configuracoesSection =  Configuration.GetSection("ConfiguracoesJWT");
            var ConfiguracoesJWT = configuracoesSection.Get<ConfiguracoesJWT>();

            services.Configure<ConfiguracoesJWT>(configuracoesSection);
            services.AddControllers().AddJsonOptions(x =>
            {
                // serialize enums as strings in api responses (e.g. Role)
                x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            //string de conexao banco
            services.AddDbContext<Context>(options => {
                
                options.UseSqlServer(Configuration.GetConnectionString("MeuSqlServer"));

            });
            services.AddScoped<IRepository, cursorepository>();
            /*services.AddAuthorization(options =>
            {
                options.AddPolicy("Gerente", policy =>
                {
                    policy.RequireAssertion(request => request.User.HasClaim(claim => claim.Type == "Gerente"));
                }
                    );
            });*/
            services.AddAuthentication(opcoes=>
            {
                opcoes.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                opcoes.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(opcoes => {
                    opcoes.TokenValidationParameters = new TokenValidationParameters {
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("QWERTYUIOP:,123456789")),
                        ValidAudience = "https://localhost:5001",
                        ValidIssuer = "Bootcamp2022",
                    };
                
                });
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ProjetoSQL", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Insira o token JWT desta maneira: Bearer Seu token",
                    Name = "Authorization",
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                }) ;

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ProjetoSQL v1"));
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
