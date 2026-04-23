using AiTalentGenome.IdentityService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AiTalentGenome.IdentityService.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<AppUser> Users => Set<AppUser>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
            entity.HasIndex(e => e.HhEmployerId).IsUnique();
        });

        modelBuilder.Entity<AppUser>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
            entity.HasIndex(e => e.HhUserId).IsUnique();

            entity.HasOne(d => d.Company)
                .WithMany(p => p.Users)
                .HasForeignKey(d => d.CompanyId);
        });
    }
}