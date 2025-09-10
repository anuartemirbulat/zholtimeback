using Constants.Enums;
using Microsoft.EntityFrameworkCore;
using RegionalRides.DAL.Entities.Entities;
using RegionalRides.DAL.Entities.References;

namespace RegionalRides.DAL;

partial class RegionalRidesContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("Orders");
            entity.HasOne(x => x.SourceAddress)
                .WithMany()
                .HasForeignKey(x => x.SourceAddressId);
            entity.HasOne(x => x.DestinationAddress)
                .WithMany()
                .HasForeignKey(x => x.DestinationAddressId);
            entity.HasOne(x => x.Customer)
                .WithMany()
                .HasForeignKey(x => x.CustomerId);
            entity.Property(x => x.State)
                .HasDefaultValue(OrderStateEnum.Created);
        });
        modelBuilder.Entity<Address>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<Address>(entity =>
        {
            entity.ToTable("Addresses");
            entity.HasOne(x => x.Kato)
                .WithMany()
                .HasForeignKey(x => x.KatoId);
        });

        modelBuilder.Entity<RefKato>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<RefKato>(entity =>
        {
            entity.ToTable("RefKatos");
            entity.HasOne(x => x.Region)
                .WithMany(x => x.RegionalChilds)
                .HasForeignKey(x => x.RegionId)
                .IsRequired(false);
            entity.HasOne(x => x.Parent)
                .WithMany(x => x.Childs)
                .HasForeignKey(x => x.ParentId)
                .IsRequired(false);
        });
        modelBuilder.Entity<Profile>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<Profile>(entity => { entity.ToTable("Profiles"); });
        OnModelCreatingPartial(modelBuilder);
    }
}
