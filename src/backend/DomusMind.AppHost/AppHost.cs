using Microsoft.Extensions.Configuration;
var builder = DistributedApplication.CreateBuilder(args);
var apiPort = builder.Configuration.GetValue("AppHost:ApiPort", 52250);
var webPort = builder.Configuration.GetValue("AppHost:WebPort", 61546);
var keycloakPort = builder.Configuration.GetValue("AppHost:KeycloakPort", 8180);
var inboxProvider = builder.Configuration["Inbox:Interpreter:Provider"] ?? "Ollama";

var postgres = builder.AddPostgres("postgres")
    .WithPgAdmin(pgAdmin => pgAdmin.WithHostPort(builder.Configuration.GetValue("AppHost:PgAdminPort", 5050)))
    .WithImageTag("17.6");
if (builder.Configuration.GetValue("AppHost:PersistData", true))
    postgres = postgres.WithDataVolume();

var domusmindDb = postgres.AddDatabase("domusmind");

var keycloak = builder.AddContainer("keycloak", "quay.io/keycloak/keycloak", "26.8.1")
    .WithArgs("start-dev", "--import-realm")
    .WithHttpEndpoint(port: keycloakPort, targetPort: 8080)
    .WithHttpHealthCheck("/realms/domusmind-dev/.well-known/openid-configuration")
    .WithBindMount(Path.GetFullPath("../../../infra/keycloak-dev"), "/opt/keycloak/data/import", isReadOnly: true)
    .WithEnvironment("KC_BOOTSTRAP_ADMIN_USERNAME", "dev-admin")
    .WithEnvironment("KC_BOOTSTRAP_ADMIN_PASSWORD", "dev-admin-only");

var api = builder.AddProject<Projects.DomusMind_Api>("api")
    .WithReference(domusmindDb)
    .WaitFor(domusmindDb)
    .WaitFor(keycloak)
    .WithEnvironment("Bff__Enabled", "true")
    .WithEnvironment("Bff__Authority", "http://localhost:" + keycloakPort + "/realms/domusmind-dev")
    .WithEnvironment("Bff__ClientId", "domusmind-dev")
    .WithEnvironment("Bff__TestUsers__00000000-0000-0000-0000-0000000000b1", "admin@domusmind.local")
    .WithEnvironment("Bff__TestUsers__00000000-0000-0000-0000-0000000000b2", "member@domusmind.local")
    .WithEnvironment("Bff__SeededTestUsers__0", "member@domusmind.local")
    .WithEnvironment("Bff__SessionLifetimeSeconds", builder.Configuration["Bff:SessionLifetimeSeconds"] ?? "1800")
    .WithEnvironment("ReverseProxy__KnownProxies__0", "127.0.0.1")
    .WithEnvironment("ReverseProxy__KnownProxies__1", "::1")
    .WithEnvironment("ReverseProxy__AllowedHosts__0", "localhost:61546")
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
    .WithEnvironment("VITE_AUTH_MODE", "oidc-bff")
    .WithEndpoint("http", endpoint =>
    {
        endpoint.Port = webPort;
    });

builder.Build().Run();
