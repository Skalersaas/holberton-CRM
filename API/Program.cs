using System.Text;
using API.Middleware;
using Persistance.Data;
using Utilities.Services;
using Microsoft.OpenApi.Models;
using Persistance.Data.Interfaces;
using Persistance.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Utilities.Security;
using Application.Interfaces;
using Application.Services;
using Swashbuckle.AspNetCore.SwaggerGen;
using Utilities.Swagger;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            ConfigureBuilder(builder);

            var app = builder.Build();

            ConfigureApp(app);

            app.Run();
        }
        private static void ConfigureBuilder(WebApplicationBuilder builder)
        {
#if DEBUG
            EnvLoader.LoadEnvFile(".env");
#endif
            // Db
            ConfigureDatabase(builder.Services);

            // Auth
            ConfigureAuthentification(builder.Services);

            // Routes
            ConfigureRoutes(builder.Services);

            // Swagger
            ConfigureSwagger(builder.Services);

            // Repos
            AddRepositories(builder.Services);
            builder.Services.AddAuthorization();
        }
        private static void ConfigureApp(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
                context.Database.Migrate();
            }
            app.UseCors(builder =>
                builder.WithOrigins("http://localhost:3000",
                                    "https://crm-dashboard-umber-sigma.vercel.app")
                       .AllowAnyMethod()
                       .AllowAnyHeader());
            app.UseRewriter(new RewriteOptions()
                .AddRedirect("^$", "swagger/index.html"));

            app.UseMiddleware<LoggingMiddleware>();

            
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection(); 


            app.MapControllers();  
        }
        #region Configures
        private static void ConfigureDatabase(IServiceCollection services)
        {
            string? cs = Environment.GetEnvironmentVariable("ConnectionString");
            services.AddDbContext<ApplicationContext>(options => options.UseNpgsql(cs));
        }
        private static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped(typeof(IModelService<,,,>), typeof(ModelService<,,,>));
        }
        private static void ConfigureAuthentification(IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = JwtTokenGenerator.Issuer,
                        ValidAudience = JwtTokenGenerator.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtTokenGenerator.SecretKey)),
                        ClockSkew = TimeSpan.Zero
                    };
                });
        }
        private static void ConfigureRoutes(IServiceCollection services)
        {
            services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
            services.AddControllers(options =>
            {
                options.Conventions.Add(new GlobalRoutePrefixConvention());
            });
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
        }
        private static void ConfigureSwagger(IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer {token}'"
                });

                options.OperationFilter<AuthRequirementFilter>();
            });
        }

        #endregion
    }
}
