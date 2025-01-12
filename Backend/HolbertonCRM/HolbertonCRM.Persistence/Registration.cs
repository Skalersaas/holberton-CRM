using HolbertonCRM.Helpers;
using HolbertonCRM.Models;
using HolbertonCRM.Persistence.Interfaces;
using HolbertonCRM.Persistence.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace HolbertonCRM.Persistence
{
    public static class Registration
    {
        public static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            EnvLoader.LoadEnvFile(".env");
            string? cs = Environment.GetEnvironmentVariable("ConnectionString");
            services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(cs));
            services.AddDataProtection();
            services.AddIdentity<AppUser, IdentityRole>(opt =>
            {
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequiredLength = 2;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireDigit = false;
                opt.SignIn.RequireConfirmedEmail = false;
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
