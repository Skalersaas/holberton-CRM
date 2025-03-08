using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Interfaces
{
    public interface IDTOSupport<T, D>
         where T : class, D, IModel
         where D : class
    {
        public static abstract T FromDTO(D dto);
    }
}
