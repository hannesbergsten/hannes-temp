using FastEndpoints.Swagger;

namespace ThreadPilot.Vehicle.Api.Initialization;

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
                    s.Title = "Vehicle API";
                    s.Description = "API for managing vehicles.";
                    s.Version = "v1";
                };
            });
        return services;
    }
}