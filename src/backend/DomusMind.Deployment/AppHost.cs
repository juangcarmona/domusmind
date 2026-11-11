var builder = DistributedApplication.CreateBuilder(args);

builder.AddDockerComposeEnvironment("compose").WithDashboard(false);

var imageTag = builder.Configuration["Image:Tag"]
    ?? throw new InvalidOperationException("Image:Tag must identify a published DomusMind image.");
var dbUser = builder.AddParameter("db-user");
var dbPassword = builder.AddParameter("db-password", secret: true);
var jwtSecret = builder.AddParameter("jwt-secret", secret: true);

var database = builder.AddPostgres("postgres", dbUser, dbPassword)
    .WithImageTag("17-alpine")
    .WithDataVolume("postgres_data")
    .PublishAsDockerComposeService((_, service) =>
    {
        service.Restart = "unless-stopped";
        service.Healthcheck = new()
        {
            Test = ["CMD-SHELL", "pg_isready -U $${POSTGRES_USER} -d domusmind"],
            Interval = "10s",
            Timeout = "5s",
            Retries = 5,
            StartPeriod = "10s"
        };
    })
    .AddDatabase("domusmind");

var ollama = builder.AddContainer("ollama", "ollama/ollama", "0.40.6")
    .WithHttpEndpoint(targetPort: 11434)
    .WithVolume("ollama_data", "/root/.ollama")
    .PublishAsDockerComposeService((_, service) => service.Restart = "unless-stopped");
var modelDownload = builder.AddContainer("ollama-model", "ollama/ollama", "0.40.6")
    .WithEntrypoint("/bin/sh")
    .WithArgs("-c", "until ollama list >/dev/null 2>&1; do sleep 2; done; ollama pull llama3.2:3b")
    .WithEnvironment("OLLAMA_HOST", "http://ollama:11434")
    .WaitFor(ollama)
    .PublishAsDockerComposeService((_, service) => service.Restart = "no");

builder.AddContainer("api", "ghcr.io/juangcarmona/domusmind", imageTag)
    .WithHttpEndpoint(port: 24365, targetPort: 8080)
    .WithExternalHttpEndpoints()
    .WithVolume("data_protection_keys", "/var/lib/domusmind/data-protection-keys")
    .WithReference(database)
    .WaitFor(database)
    .WaitFor(modelDownload)
    .WithEnvironment("ASPNETCORE_ENVIRONMENT", "Production")
    .WithEnvironment("DataProtection__KeysPath", "/var/lib/domusmind/data-protection-keys")
    .WithEnvironment("Inbox__Interpreter__Provider", "Ollama")
    .WithEnvironment("OLLAMA_LLAMA3_2_URI", "http://ollama:11434")
    .WithEnvironment("OLLAMA_LLAMA3_2_MODEL", "llama3.2:3b")
    .WithEnvironment("Jwt__SigningKey", jwtSecret)
    .WithEnvironment("Jwt__Issuer", "domusmind")
    .WithEnvironment("Jwt__Audience", "domusmind")
    .WithEnvironment("Swagger__Enabled", "false")
    .PublishAsDockerComposeService((_, service) =>
    {
        service.Restart = "unless-stopped";
        service.DependsOn["postgres"].Condition = "service_healthy";
        service.DependsOn["ollama-model"].Condition = "service_completed_successfully";
    });

builder.Build().Run();
