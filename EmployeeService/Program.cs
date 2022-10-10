using AutoMapper;
using EmployeeService.Services.Repositories;
using EmployeeService.Services.Repositories.Impl;
using EmployeeServiceData;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using NLog.Web;

namespace EmployeeService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            #region Configure Automapper

            // Create mapper configuration from our class MapperProfile
            var mapperConfiguration = new MapperConfiguration(mp => mp.AddProfile(new MapperProfile()));

            // Create mapper from our configuration
            var mapper = mapperConfiguration.CreateMapper();

            // Register our mapper with the app as singletone 
            builder.Services.AddSingleton(mapper);

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

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();
            app.UseHttpLogging();

            app.MapControllers();

            app.Run();
        }
    }
}