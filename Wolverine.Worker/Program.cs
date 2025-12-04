using Wolverine;
using Wolverine.Worker.Orders;
using Wolverine.Kafka;
using Wolverine.Worker;

var builder = Host.CreateApplicationBuilder(args);

builder.UseWolverine(opts =>
{
    var kafkaSettings = builder.Configuration
                           .GetSection(nameof(Kafka))
                           .Get<Kafka>()
                       ?? throw new InvalidOperationException(
                           $"As configurações do Kafka ({nameof(Kafka)}) não foram encontradas."
                       );

    var connString = builder.Configuration.GetConnectionString("kafka");

    opts.UseKafka(connString)
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

