using System.Reflection;
using Porcupine.Application.Common.Interfaces;
using Porcupine.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Porcupine.Domain.Entities;
using Microsoft.AspNetCore.Http.Features;

namespace Porcupine.Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
    : IdentityDbContext<ApplicationUser>(options), IApplicationDbContext
{
    public DbSet<Industry> Industries => Set<Industry>();
    public DbSet<MarketSegment> MarketSegments => Set<MarketSegment>();
    public DbSet<LifecycleStage> LifecycleStages => Set<LifecycleStage>();
    public DbSet<Organization> Organizations => Set<Organization>();
    public DbSet<Rule> Rules => Set<Rule>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
