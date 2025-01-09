using HolbertonCRM.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HolbertonCRM.Persistence.Interfaces
{
    public interface IUnitOfWork
    {
        void Commit();
    }
}
