using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Core.Systems
{
    [ExcludeFromCodeCoverage]

    public class SecurityRequirementsOperationFilter(
    ) : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var metadata = context.ApiDescription.ActionDescriptor?.EndpointMetadata;

            //check if allow anonymous
            bool isAllowAnonymous = metadata?.Any(m => m is AllowAnonymousAttribute) == true;

            // Check if Authorize is present at action or controller level
            var hasAuthorize = metadata.Any(a => a is AuthorizeAttribute);

            if (hasAuthorize && !isAllowAnonymous)
            {
                // Add security requirement for endpoints that need authorization
                operation.Security = new List<OpenApiSecurityRequirement>
                {
                    // new OpenApiSecurityRequirement
                    // {
                    //     {
                    //         [new OpenApiSecuritySchemeReference("Cookie")] = []
                    //     }
                    // }
                };
            }
        }
    }
}
