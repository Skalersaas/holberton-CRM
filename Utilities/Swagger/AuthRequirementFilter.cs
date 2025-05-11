using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Utilities.Swagger
{
    /// <summary>
    /// Implements IOperationFilter to add security requirements to Swagger/OpenAPI documentation.
    /// This filter automatically adds Bearer token authentication requirements to endpoints that have the [Authorize] attribute.
    /// </summary>
    public class AuthRequirementFilter : IOperationFilter
    {
        /// <summary>
        /// Applies security requirements to the OpenAPI operation based on authorization attributes.
        /// </summary>
        /// <param name="operation">The OpenAPI operation to modify.</param>
        /// <param name="context">The current operation filter context.</param>
        /// <remarks>
        /// Checks for [Authorize] attributes and adds Bearer token security requirements, or clears them if no authorization is needed.
        /// </remarks>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var methodInfo = context.MethodInfo;
            var declaringType = methodInfo.DeclaringType;

            var hasAuthorize =
                declaringType?.GetCustomAttributes(typeof(AuthorizeAttribute), true).Length != 0 == true &&
                methodInfo.GetCustomAttributes(typeof(AllowAnonymousAttribute), true).Length == 0;

            if (!hasAuthorize)
            {
                operation.Security.Clear();
                return;
            }
            operation.Security =
            [
                new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                }
            ];
        }
    }
}
