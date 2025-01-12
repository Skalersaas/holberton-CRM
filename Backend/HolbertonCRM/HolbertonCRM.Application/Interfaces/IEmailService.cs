using HolbertonCRM.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HolbertonCRM.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendAsync(EmailMember emailMember);
    }
}
