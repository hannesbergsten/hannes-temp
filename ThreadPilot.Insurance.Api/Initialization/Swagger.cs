using FastEndpoints.Swagger;

namespace ThreadPilot.Insurance.Api.Initialization;

public static class SwaggerInitExtensions
{
    /// Adds Swagger documentation to the service collection.
    public static IServiceCollection AddAppSwagger(this IServiceCollection services)
    {
        services
            .SwaggerDocument(o =>
            {
                o.MaxEndpointVersion = 1;
                o.DocumentSettings = s =>
                {
                    s.DocumentName = "v1";
                    s.Title = "Insurance API";
                    s.Description = "API for managing insurances.";
                    s.Version = "v1";
                };
            });
        return services;
    }
}