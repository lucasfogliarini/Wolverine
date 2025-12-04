#:sdk Aspire.AppHost.Sdk@13.0.2
#:package Aspire.Hosting.Kafka@13.0.1

using Aspire.Hosting;
using Microsoft.Extensions.DependencyInjection;

var builder = DistributedApplication.CreateBuilder(args);

builder.Configuration["ASPIRE_ALLOW_UNSECURED_TRANSPORT"] = "true";
builder.Configuration["ASPIRE_DASHBOARD_OTLP_HTTP_ENDPOINT_URL"] = "http://localhost:2007";
builder.Configuration["ASPNETCORE_URLS"] = "http://localhost:2006";

var kafka = builder.AddKafka("kafka")
    .WithKafkaUI()
    .WithLifetime(ContainerLifetime.Persistent)
    .WithDataVolume("aspire_kafka_data");

var wolverine = builder.AddProject("wolverine", "Wolverine")
    .WaitFor(kafka)
    .WithReference(kafka);

var app = builder.Build();

await app.RunAsync();