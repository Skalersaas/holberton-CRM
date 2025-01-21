using API.Middleware;
using Microsoft.EntityFrameworkCore;
using Persistance.Data;
using Persistance.Data.Repositories;
using Utilities.Services;


namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Db
            ConfigureDatabase(builder.Services);

            // Repos
            AddRepositories(builder.Services);

            builder.Services.AddControllers(options =>
            {
                options.Conventions.Add(new GlobalRoutePrefixConvention());
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            //app.UseMiddleware<RequestLoggingMiddleware>();
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
        public static void ConfigureDatabase(IServiceCollection services)
        {
            EnvLoader.LoadEnvFile(".env");
            string? cs = Environment.GetEnvironmentVariable("ConnectionString");
            services.AddDbContext<ApplicationContext>(options => options.UseNpgsql(cs));
        }
        public static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<AdmissionManagement>();
        }
    }
}
