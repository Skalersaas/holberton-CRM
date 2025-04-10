using Domain.Models.JsonTemplates;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Services
{
    public static class ModelErrorExtractor
    {
        public static List<FieldError> ExtractFieldErrors(ModelStateDictionary modelState)
        {
            return modelState
                .Where(x => x.Value.Errors.Count > 0)
                .SelectMany(x => x.Value.Errors.Select(error => new FieldError
                {
                    Field = x.Key,
                    Error = error.ErrorMessage
                }))
                .ToList();
        }
    }
}
