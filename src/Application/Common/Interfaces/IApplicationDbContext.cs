using Porcupine.Domain.Entities;

namespace Porcupine.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    public DbSet<Industry> Industries { get; }
    public DbSet<MarketSegment> MarketSegments { get; }
    public DbSet<LifecycleStage> LifecycleStages { get; }
    public DbSet<Organization> Organizations { get; }
    public DbSet<Rule> Rules { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
