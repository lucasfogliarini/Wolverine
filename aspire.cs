#:sdk Aspire.AppHost.Sdk@13.0.0

using Aspire.Hosting;
using Microsoft.Extensions.DependencyInjection;

var builder = DistributedApplication.CreateBuilder(args);

builder.Configuration["ASPIRE_ALLOW_UNSECURED_TRANSPORT"] = "true";
builder.Configuration["ASPIRE_DASHBOARD_OTLP_HTTP_ENDPOINT_URL"] = "http://localhost:2007";
builder.Configuration["ASPNETCORE_URLS"] = "http://localhost:2006";


var app = builder.Build();

await app.RunAsync();