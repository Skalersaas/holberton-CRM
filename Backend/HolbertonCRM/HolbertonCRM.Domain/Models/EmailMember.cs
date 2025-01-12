using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HolbertonCRM.Domain.Models
{
    public class EmailMember
    {
        public string to;
        public string body;
        public string subject;
        public string? link;
        public string OTP;
    }
}
