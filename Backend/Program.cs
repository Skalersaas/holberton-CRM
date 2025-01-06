using holberton_CRM.Data;
using holberton_CRM.Helpers;
using holberton_CRM.Middleware;
using Microsoft.EntityFrameworkCore;

namespace holberton_CRM
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            ConfigureDatabase(builder);
            // Add services to the container.
            builder.Services.AddControllers(options =>
            {
                options.Conventions.Add(new GlobalRoutePrefixConvention()); 
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();
            
            app.UseRouting();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
        public static void ConfigureDatabase(WebApplicationBuilder builder)
        {
            EnvLoader.LoadEnvFile(".env");
            string? cs = Environment.GetEnvironmentVariable("ConnectionString");
            builder.Services.AddDbContext<ApplicationContext>(options => options.UseNpgsql(cs));
        }
    }
}
