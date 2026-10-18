using Microsoft.Extensions.Configuration;
var builder = DistributedApplication.CreateBuilder(args);
var apiPort = builder.Configuration.GetValue("AppHost:ApiPort", 52250);
var webPort = builder.Configuration.GetValue("AppHost:WebPort", 61546);
var inboxProvider = builder.Configuration["Inbox:Interpreter:Provider"] ?? "Ollama";

var postgres = builder.AddPostgres("postgres")
    .WithPgAdmin(pgAdmin => pgAdmin.WithHostPort(builder.Configuration.GetValue("AppHost:PgAdminPort", 5050)))
    .WithImageTag("17.6")
    .WithDataVolume();

var domusmindDb = postgres.AddDatabase("domusmind");

var api = builder.AddProject<Projects.DomusMind_Api>("api")
    .WithReference(domusmindDb)
    .WaitFor(domusmindDb)
    .WithEnvironment("Inbox__Interpreter__Provider", inboxProvider)
    .WithEnvironment("Inbox__Interpreter__Endpoint", builder.Configuration["Inbox:Interpreter:Endpoint"] ?? "")
    .WithEnvironment("Inbox__Interpreter__Model", builder.Configuration["Inbox:Interpreter:Model"] ?? "")
    .WithEnvironment("Inbox__Interpreter__ApiKey", builder.Configuration["Inbox:Interpreter:ApiKey"] ?? "")
    .WithEnvironment("DevelopmentTelemetry__OutboundProbeUrl", "http://localhost:61546/")
    .WithEndpoint("http", endpoint =>
    {
        endpoint.Port = apiPort;
    });

if (inboxProvider == "Ollama")
{
    var ollama = builder.AddOllama("ollama").WithImageTag("0.40.6").WithDataVolume();
    var inboxModel = ollama.AddModel(builder.Configuration["Inbox:Interpreter:Model"] ?? "llama3.2:3b");
    api = api.WithReference(inboxModel).WaitFor(inboxModel);
}

builder.AddViteApp("web-app", "../../web/app")
    .WithReference(api)
    .WaitFor(api)
    .WithEndpoint("http", endpoint =>
    {
        endpoint.Port = webPort;
    });

builder.Build().Run();
