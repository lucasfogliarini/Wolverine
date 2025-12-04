using Confluent.Kafka;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using Wolverine;
using Wolverine.Kafka;
using Wolverine.Worker;
using Wolverine.Worker.Orders;

namespace Microsoft.Extensions.DependencyInjection;
public static class DependencyInjection
{
    public static void AddWorker(this IHostApplicationBuilder builder)
    {
        builder.AddOpenTelemetryExporter();
        builder.UseWolverineFx();
    }

    private static void UseWolverineFx(this IHostApplicationBuilder builder)
    {
        builder.UseWolverine(opts =>
        {
            var kafkaSettings = builder.Configuration
                                    .GetSection(nameof(Kafka))
                                    .Get<Kafka>()
                                ?? throw new InvalidOperationException(
                                    $"As configurações do Kafka ({nameof(Kafka)}) não foram encontradas."
                                );

            var connString = builder.Configuration.GetConnectionString("kafka");

            opts.UseKafka(connString);

            opts.ListenToKafkaTopic(kafkaSettings.TopicOrderCreated)
                .ReceiveRawJson<OrderCreated>();
                //.ConfigureConsumer(config =>
                //{
                //    config.GroupId = kafkaSettings.GroupId;
                //    config.AutoOffsetReset = AutoOffsetReset.Latest;
                //});

            opts.PublishMessage<OrderProcessed>()
                .ToKafkaTopic(kafkaSettings.TopicOrderProcessed);
        });
    }

    private static void AddOpenTelemetryExporter(this IHostApplicationBuilder builder)
    {
        builder.Services.AddOpenTelemetry()
            .WithTracing(tracerBuilder =>
            {
                tracerBuilder.AddOtlpExporter();
            })
            .WithMetrics(meterBuilder =>
            {
                meterBuilder.AddOtlpExporter();
            })
            .WithLogging(loggingBuilder =>
            {
                loggingBuilder.AddOtlpExporter();
            });

        builder.Logging.AddOpenTelemetry(options =>
        {
            options.IncludeFormattedMessage = true;
            options.IncludeScopes = true;
            options.ParseStateValues = true;
        });
    }
}
