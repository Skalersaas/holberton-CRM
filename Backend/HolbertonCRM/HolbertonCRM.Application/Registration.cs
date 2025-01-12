using FluentValidation;
using HolbertonCRM.Application.Interfaces;
using HolbertonCRM.Application.Mapper;
using HolbertonCRM.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HolbertonCRM.Application
{
    public static class Registration
    {
        public static void AddApplication(this IServiceCollection services) 
        {
            var assembly = Assembly.GetExecutingAssembly();

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IAdmissionService, AdmissionService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddAutoMapper(option =>
            {
                option.AddProfile<MapProfile>();
            });
            services.AddValidatorsFromAssembly(assembly);
        }
    }
}
