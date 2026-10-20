using System.Reflection.PortableExecutable;
using System.Reflection.Metadata;
using DomusMind.Mcp.DependencyInjection;

namespace DomusMind.Api.Tests.Mcp;

public sealed class McpDependencyBoundaryTests
{
    [Fact]
    public void Project_declares_only_the_allowed_application_and_contract_dependencies()
    {
        var projectPath = Path.Combine(FindRepositoryRoot(), "src", "backend", "DomusMind.Mcp", "DomusMind.Mcp.csproj");
        var project = System.Xml.Linq.XDocument.Load(projectPath);
        var projectReferences = project.Descendants("ProjectReference")
            .Select(element => Path.GetFileNameWithoutExtension(((string?)element.Attribute("Include"))?.Replace('\\', '/') ?? string.Empty))
            .Order(StringComparer.Ordinal)
            .ToArray();
        var packageReferences = project.Descendants("PackageReference")
            .Select(element => (string?)element.Attribute("Include") ?? string.Empty)
            .Order(StringComparer.Ordinal)
            .ToArray();

        Assert.Equal(["DomusMind.Application", "DomusMind.Contracts"], projectReferences);
        Assert.Equal(["ModelContextProtocol.AspNetCore"], packageReferences);
    }

    [Fact]
    public void Compiled_adapter_does_not_reference_domain_or_persistence_types()
    {
        using var stream = File.OpenRead(typeof(DomusMindMcpServiceCollectionExtensions).Assembly.Location);
        using var peReader = new PEReader(stream);
        var metadata = peReader.GetMetadataReader();
        var assemblyReferences = metadata.AssemblyReferences
            .Select(handle => metadata.GetString(metadata.GetAssemblyReference(handle).Name))
            .ToArray();
        var typeReferences = metadata.TypeReferences
            .Select(handle =>
            {
                var reference = metadata.GetTypeReference(handle);
                var @namespace = metadata.GetString(reference.Namespace);
                var name = metadata.GetString(reference.Name);
                return string.IsNullOrEmpty(@namespace) ? name : $"{@namespace}.{name}";
            })
            .ToArray();

        Assert.DoesNotContain(assemblyReferences, name =>
            name.Contains("Infrastructure", StringComparison.OrdinalIgnoreCase) ||
            name.Contains("Domain", StringComparison.OrdinalIgnoreCase) ||
            name.Contains("EntityFrameworkCore", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(typeReferences, type =>
            type == "DomusMind.Application.Abstractions.Persistence.IDomusMindDbContext" ||
            type.StartsWith("DomusMind.Domain.", StringComparison.Ordinal) ||
            type.StartsWith("DomusMind.Infrastructure.", StringComparison.Ordinal) ||
            type.StartsWith("Microsoft.EntityFrameworkCore.", StringComparison.Ordinal) ||
            type.Contains("Persistence", StringComparison.OrdinalIgnoreCase));
    }

    private static string FindRepositoryRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null && !File.Exists(Path.Combine(directory.FullName, "DomusMind.slnx")))
        {
            directory = directory.Parent;
        }

        return directory?.FullName ?? throw new DirectoryNotFoundException("Could not locate DomusMind.slnx from the test output directory.");
    }
}
