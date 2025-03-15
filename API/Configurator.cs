using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;

using Persistence.DB;
using API.Middleware;
using Application.Services;
using Persistence.Repositories;
namespace API
{
    public static class Configurator
    {
        public static void ConfigureServices(this WebApplicationBuilder builder)
        {
            ConfigureDatabase(builder.Services);
            ConfigureAuthentification(builder.Services);
            ConfigureRoutes(builder.Services);
            ConfigureSwagger(builder.Services);
            AddRepositories(builder.Services);
            builder.Services.AddHealthChecks();
        }
        public static void ConfigureMiddleware(this WebApplication app)
        {
            app.UseRewriter(new RewriteOptions()
                .AddRedirect("^$", "swagger/index.html"));

            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseMiddleware<RateLimitingMiddleware>();
            app.UseMiddleware<LoggingMiddleware>();

            app.UseHealthChecks("/health");
            app.UseResponseCaching();
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
                        ValidIssuer = JwtGenerator.Issuer,
                        ValidAudience = JwtGenerator.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtGenerator.SecretKey)),
                        ClockSkew = TimeSpan.Zero
                    };
                });
        }
        private static void ConfigureRoutes(IServiceCollection services)
        {
            services.AddControllers();
            services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
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
