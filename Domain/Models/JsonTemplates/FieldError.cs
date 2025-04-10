using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.JsonTemplates
{
    public class FieldError
    {
        public string Field { get; set; } = null!;
        public string Error { get; set; } = null!;
    }
}
