namespace DomusMind.Application.Abstractions.Persistence;

public interface IDomusMindDbContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}