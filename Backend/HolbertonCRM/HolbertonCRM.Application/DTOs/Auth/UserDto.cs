using HolbertonCRM.Utilities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HolbertonCRM.Application.DTOs.Auth
{
    public class UserDto
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; } 
        public UserRole Role { get; set; } 
        public bool IsAdmin { get; set; } 
    }
}
