using Wolverine;
using Wolverine.Orders;
using Wolverine.Kafka;

var builder = Host.CreateApplicationBuilder(args);

builder.UseWolverine(opts =>
{
    var kafkaSettings = builder.Configuration
                           .GetSection(nameof(KafkaSettings))
                           .Get<KafkaSettings>()
                       ?? throw new InvalidOperationException(
                           $"As configurações do Kafka ({nameof(KafkaSettings)}) não foram encontradas."
                       );

    // Configure Kafka transport
    opts.UseKafka(kafkaSettings.BootstrapServers)
        .ConfigureClient(client =>
        {
            client.ClientId = kafkaSettings.GroupId;
        });

    opts.ListenToKafkaTopic(kafkaSettings.TopicOrderCreated)
        .ProcessInline();

    opts.PublishMessage<OrderProcessed>()
        .ToKafkaTopic(kafkaSettings.TopicOrderProcessed);
    
});

var host = builder.Build();
host.Run();

