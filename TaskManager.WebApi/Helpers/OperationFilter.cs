using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

public class AddRequiredHeaderParameter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // Перевіряємо, чи є атрибут RequireUserIdHeaderAttribute на класі або методі
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
            Description = "ID користувача, який виконує запит",
            Schema = new OpenApiSchema
            {
                Type = "integer",
                Default = new Microsoft.OpenApi.Any.OpenApiInteger(1)
            }
        });
    }
}