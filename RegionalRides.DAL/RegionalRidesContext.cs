using Microsoft.EntityFrameworkCore;
using RegionalRides.DAL.Entities.Entities;
using RegionalRides.DAL.Entities.References;

namespace RegionalRides.DAL;

public partial class RegionalRidesContext : DbContext, IDisposable
{
    public RegionalRidesContext() : base()
    {
    }

    public async Task DisposeAsync()
    {
        await base.DisposeAsync();
    }

    public void Dispose()
    {
        base.Dispose();
    }

    public void RollBack()
    {
        base.Database.RollbackTransaction();
    }

    public RegionalRidesContext(DbContextOptions<RegionalRidesContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLazyLoadingProxies();
        optionsBuilder.EnableDetailedErrors();
        optionsBuilder.EnableSensitiveDataLogging();
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        PreSaveChanges();
        return await base.SaveChangesAsync(true, cancellationToken);
    }

    public override int SaveChanges()
    {
        PreSaveChanges();
        return base.SaveChanges();
    }

    protected virtual void PreSaveChanges()
    {
        foreach (var entry in ChangeTracker.Entries())
            switch (entry.State)
            {
                case EntityState.Added when entry.Entity is BaseEntity:
                {
                    entry.Property(nameof(BaseEntity.DateCreate)).CurrentValue = DateTime.Now;
                    entry.Property(nameof(BaseEntity.DateUpdate)).CurrentValue = DateTime.Now;
                    break;
                }
                case EntityState.Deleted when entry.Entity is BaseEntity:
                {
                    entry.State = EntityState.Unchanged;
                    entry.Property(nameof(BaseEntity.IsDeleted)).CurrentValue = true;
                    entry.Property(nameof(BaseEntity.DateUpdate)).CurrentValue = DateTime.Now;
                    break;
                }
                case EntityState.Modified when entry.Entity is BaseEntity:
                {
                    entry.Property(nameof(BaseEntity.DateUpdate)).CurrentValue = DateTime.Now;
                    break;
                }
            }
    }

    public virtual DbSet<RefKato> RefKatos { get; set; }
    public virtual DbSet<Address> Addresses { get; set; }
    public virtual DbSet<Order> Orders { get; set; }
    public virtual DbSet<Profile> Profiles { get; set; }
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
