using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace ProductService.OpenTelemetry;

public static class OpenTelemetryConfiguration
{
    public static void AddOpenTelemetryServices(this IServiceCollection services, IConfiguration configuration)
    {
        var serviceName = "ProductService";
        var serviceVersion = "1.0.0";

        var resourceBuilder = ResourceBuilder.CreateDefault()
            .AddService(serviceName: serviceName, serviceVersion: serviceVersion);

        services.AddOpenTelemetry()
            .ConfigureResource(r => r.AddService(serviceName: serviceName, serviceVersion: serviceVersion))
            .WithTracing(tracerProviderBuilder => tracerProviderBuilder
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .SetResourceBuilder(resourceBuilder)
            )
            .WithMetrics(metricProviderBuilder => metricProviderBuilder
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddRuntimeInstrumentation()
                    .AddPrometheusExporter() 
            );
    }
}