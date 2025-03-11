using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.JsonTemplates
{
    public class SortModel
    {
        public string Field { get; set; } = string.Empty;
        public bool IsAscending { get; set; }
    }
}
