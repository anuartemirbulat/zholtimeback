using Identity.DAL.Entities.Entities;
using Microsoft.EntityFrameworkCore;

namespace Identity.DAL;

partial class IdentityDbContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AccountConfirmation>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<AccountConfirmation>(entity => { entity.ToTable("AccountConfirmations"); });

        modelBuilder.Entity<User>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<User>(entity => { entity.ToTable("Users"); });

        modelBuilder.Entity<Profile>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<Profile>(entity =>
        {
            entity.ToTable("Profiles");
            entity.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId);
            entity.HasOne(x => x.Role)
                .WithMany()
                .HasForeignKey(x => x.RoleId);
        });

        modelBuilder.Entity<RefRole>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<RefRole>(entity => { entity.ToTable("RefRoles"); });

        OnModelCreatingPartial(modelBuilder);
    }
}
