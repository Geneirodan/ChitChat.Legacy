using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

namespace Common.OpenTelemetry;

public static class DependencyInjection
{
    public static IHostApplicationBuilder AddOpenTelemetry(this IHostApplicationBuilder builder)
    {
        builder.Logging.AddOpenTelemetry(x =>
        {
            x.IncludeScopes = true;
            x.IncludeFormattedMessage = true;
        });
        builder.Services.AddOpenTelemetry().WithMetrics(x => x.AddRuntimeInstrumentation()
            .AddMeter(
                "Microsoft.AspNetCore.Hosting",
                "Microsoft.AspNetCore.Server.Kestrel",
                "System.Net.Http"
            )).WithTracing(x =>
        {
            if (builder.Environment.IsDevelopment())
                x.SetSampler<AlwaysOnSampler>();
            x.AddAspNetCoreInstrumentation().AddHttpClientInstrumentation();
        });
        builder.AddOpenTelemetryExporters();
        return builder;
    }

    private static void AddOpenTelemetryExporters(this IHostApplicationBuilder builder)
    {
        builder.Services.Configure<OpenTelemetryLoggerOptions>(x => x.AddOtlpExporter());
        builder.Services.ConfigureOpenTelemetryMeterProvider(x => x.AddOtlpExporter());
        builder.Services.ConfigureOpenTelemetryTracerProvider(x => x.AddOtlpExporter());
    }

    public static WebApplication MapHealthChecks(this WebApplication app)
    {
        app.MapHealthChecks("/health");
        app.MapHealthChecks("/alive", new HealthCheckOptions { Predicate = r => r.Tags.Contains("live") });
        return app;
    }
}