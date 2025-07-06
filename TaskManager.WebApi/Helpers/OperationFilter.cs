using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

public class AddRequiredHeaderParameter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // Проверяем, есть ли атрибут на методе или контроллере
        var hasAttribute = context.MethodInfo.DeclaringType?.GetCustomAttributes(true).OfType<RequireUserIdHeaderAttribute>().Any() == true
            || context.MethodInfo.GetCustomAttributes(true).OfType<RequireUserIdHeaderAttribute>().Any();

        if (!hasAttribute)
            return;

        operation.Parameters ??= new List<OpenApiParameter>();

        operation.Parameters.Add(new OpenApiParameter
        {
            Name = "User-Id",
            In = ParameterLocation.Header,
            Required = true,
            Schema = new OpenApiSchema
            {
                Type = "integer",
                Default = new Microsoft.OpenApi.Any.OpenApiInteger(1)
            }
        });
    }
}