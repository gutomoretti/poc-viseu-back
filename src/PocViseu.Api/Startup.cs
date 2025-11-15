using PocViseu.Api.Jwt;
using PocViseu.Api.Jwt.Interfaces;
using PocViseu.Api.Middleware;
using PocViseu.Api.Services;
using PocViseu.Api.Services.Interfaces;
using PocViseu.Infrastructure.Database;
using PocViseu.Model.Bussines;
using PocViseu.Model.Config;
using PocViseu.Model.Extensions;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json;
using WebControlCorp.Api.Controllers;

namespace PocViseu.Api
{
    public class Startup
    {
        public const string apiCorsPolicy = "ApiCorsPolicy";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: apiCorsPolicy,
                                  builder =>
                                  {
                                      builder.AllowAnyOrigin()
                                        .AllowAnyHeader()
                                        .AllowAnyMethod();
                                      //.WithMethods("OPTIONS", "GET");
                                  });
            });

            services.AddControllers()
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = context =>
                    {
                        // Pega o body do JSON que foi salvo pelo Middleware
                        var requestBody = context.HttpContext.Items["RequestBody"] as string;

                        var _logSystemService = context.HttpContext.RequestServices.GetRequiredService<ILogSystemService>();
                        _ = _logSystemService.Log("RequestBody".ToLog(requestBody, LogLevelStatus.High, 1, Guid.NewGuid().ToString()));

                        // Pega o serviço de logger
                        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<ServiceBot>>();

                        // Itera sobre os erros de validação
                        foreach (var modelStateKey in context.ModelState.Keys)
                        {
                            var value = context.ModelState[modelStateKey];

                            // Verifica se o erro é de desserialização de JSON
                            if (value.Errors.Any(e => e.Exception is JsonException))
                            {
                                logger.LogError("Erro de desserialização JSON para o campo '{campo}'. JSON: {jsonBody}. Erro: {erro}",
                                    modelStateKey,
                                    requestBody,
                                    value.Errors.FirstOrDefault()?.ErrorMessage);

                                // Uma vez que o erro de JSON é encontrado, não precisamos continuar.
                                break;
                            }
                        }

                        var problemDetails = new ValidationProblemDetails(context.ModelState)
                        {
                            Status = StatusCodes.Status400BadRequest,
                            Title = "Erro de validação",
                            Detail = "Um ou mais erros de validação ocorreram."
                        };

                        return new BadRequestObjectResult(problemDetails);
                    };
                });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            var paramEnv = Configuration.GetValue<string>("DbConnection");

            const int commandTimeoutInSeconds = 120;

            services.AddDbContext<WebControlDbContext>(options =>
                                      options.UseMySQL(paramEnv, options =>
                                      {
                                          options.CommandTimeout(commandTimeoutInSeconds);
                                      }));


            //JWT
            var tokenKey = Configuration.GetValue<string>("TokenKey");
            var key = Encoding.ASCII.GetBytes(tokenKey);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                };
            });
            services.AddSingleton<ITokenRefresher>(x =>
                            new TokenRefresher(key, x.GetService<IJWTAuthenticationManager>()));
            services.AddSingleton<IRefreshTokenGenerator, RefreshTokenGenerator>();
            services.AddSingleton<IJWTAuthenticationManager>(x =>
                new JWTAuthenticationManager(tokenKey, x.GetService<IRefreshTokenGenerator>()));

            services.AddSingleton<ILogSystemService, LogSystemService>();
            services.AddSingleton<IMailService, MailService>();
           
            services.AddHttpClient<IHttpBot, HttpBot>(client =>
            {
                client.BaseAddress = new Uri(Configuration["UrlBot"]);
            }).SetHandlerLifetime(TimeSpan.FromMinutes(5));


            #region swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PocViseu", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."

                });
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
                         new string[] {}
                    }
                });
            });
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var salesContext = scope.ServiceProvider.GetRequiredService<WebControlDbContext>();
                //salesContext.Database.Migrate();
                salesContext.Database.EnsureCreated();
                salesContext.Seed();
            }

            //Load SYSTEM_TIME_ZONE
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var wcContext = scope.ServiceProvider.GetRequiredService<WebControlDbContext>();
                //PATH_BOT
                var queryParamBotPath = from p in wcContext.WebcorpConfig!.Where(x => x.Excluido == false && x.ParamKey == "SYSTEM_TIME_ZONE")
                                  .OrderBy(x => x.Id)
                                 .ToList()
                                        select p;
                var itemParamBotParam = queryParamBotPath.FirstOrDefault();
                if (itemParamBotParam != null)
                {
                    SysConfig.TMZ = int.Parse(itemParamBotParam.ParamValue ?? "0");
                }
            }

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseMiddleware<RequestBodyLoggingMiddleware>();

            app.UseRouting();

            app.UseAuthentication();

            app.UseCors(apiCorsPolicy);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


        }
    }
}
