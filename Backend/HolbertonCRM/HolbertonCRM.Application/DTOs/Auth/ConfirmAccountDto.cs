using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HolbertonCRM.Application.DTOs.Auth
{
    public class ConfirmAccountDto
    {
        public string Email { get; set; }
        public string OTP { get; set; }
    }
}
