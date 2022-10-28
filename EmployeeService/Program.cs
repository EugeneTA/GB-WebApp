using AutoMapper;
using EmployeeService.Models.Dto;
using EmployeeService.Models.Requests;
using EmployeeService.Models.Validators;
using EmployeeService.Services;
using EmployeeService.Services.gRPCServices;
using EmployeeService.Services.Repositories;
using EmployeeService.Services.Repositories.Impl;
using EmployeeServiceData;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog.Web;
using System.Net;
using System.Text;

namespace EmployeeService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.WebHost.ConfigureKestrel(options =>
            {
                options.Listen(IPAddress.Any, 5001, listenOptions =>
                {
                    listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http1AndHttp2;
                    listenOptions.UseHttps(@"c:\gbtestcert.pfx", "12345");
                });
            });

            // Add services to the container.
            #region Configure Automapper

            // Create mapper configuration from our class MapperProfile
            var mapperConfiguration = new MapperConfiguration(mp => mp.AddProfile(new MapperProfile()));

            // Create mapper from our configuration
            var mapper = mapperConfiguration.CreateMapper();

            // Register our mapper with the app as singletone 
            builder.Services.AddSingleton(mapper);

            #endregion

            #region Configure gRPC

            // 1. добавляем поддержк уработы с gRPC
            builder.Services.AddGrpc();

            #endregion

            #region Configure Logging Services

            builder.Services.AddHttpLogging(logging =>
            {
                logging.LoggingFields = HttpLoggingFields.All | HttpLoggingFields.RequestQuery;
                logging.RequestBodyLogLimit = 4096;
                logging.ResponseBodyLogLimit = 4096;
                logging.RequestHeaders.Add("Authorization");
                logging.RequestHeaders.Add("X-Real-IP");
                logging.RequestHeaders.Add("X-Forwarded-For");
            });

            builder.Host.ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddConsole();

            }).UseNLog(new NLogAspNetCoreOptions() { RemoveLoggerFactoryFilter = true });

            #endregion

            #region Configure Repositories

            builder.Services.AddScoped<IEmployeeTypeRepository, EmployeeTypeRepository>();
            builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

            #endregion

            #region Configure EF DBContext Service (EmployeeDatabase Database)

            builder.Services.AddDbContext<EmployeeServiceDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration["Settings:DatabaseOptions:ConnectionString"]);
            }
            );

            #endregion

            #region Configure Authenticate 

            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme =
                JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme =
                JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new
                TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(AuthenticateService.SecretKey)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };
            });

            #endregion

            #region Configure FluentValidator

            // Проверка поступающих данных в запросе
            builder.Services.AddScoped<IValidator<AuthenticationRequest>, AuthenticationRequestValidator>();
            builder.Services.AddScoped<IValidator<EmployeeCreateRequest>, EmployeeCreateRequestValidator>();
            builder.Services.AddScoped<IValidator<EmployeeDto>, EmployeeValidator>();

            #endregion

            builder.Services.AddSingleton<IAuthenticateService, AuthenticateService>();


            // Добавляем поодержку работы с контроллерами
            builder.Services.AddControllers();


            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "GB WebApp Course", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Description = "JWT Authorization header using the Bearer scheme(Example: 'Bearer 12345abcdef')",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme()
                        {
                            Reference = new OpenApiReference()
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });

            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            // 3.1 Стандартный HTTP логгер не работает с gGRPC. Будет ошибка.
            //app.UseHttpLogging();

            // 3.2 Обещали поправить в .Net 7
            app.UseWhen(
                ctx => ctx.Request.ContentType != "application/grpc",
                builder =>
                {
                    builder.UseHttpLogging();

                }
                );

            app.MapControllers();

            // 2. Добавляем gRPC сервисы
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<DictionariesService>();
                endpoints.MapGrpcService<DepartmentsService>();
                endpoints.MapGrpcService<EmployeesService>();
            });

            app.Run();
        }
    }
}