namespace Wolverine.Worker;

public record Kafka(string TopicOrderCreated, string TopicOrderProcessed, string GroupId);