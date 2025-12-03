using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Shared
{
    public abstract class ADbContext: DbContext
    {
        protected ADbContext(DbContextOptions options): base(options) { }

        // Example DbSet - replace or extend with your own entities
        public DbSet<FamilyEntity> FamilyTable { get; set; }
        public DbSet<UserEntity> UserTable { get; set; }
        public DbSet<UserCredentialsEntity> CredentialsTable { get; set; }
        public DbSet<UserSettingsEntity> SettingsTable { get; set; }
        public DbSet<LogMessageEntity> LogMessageTable { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure one-to-many: One Family has many Users. A User belongs to one Family.
            modelBuilder.Entity<FamilyEntity>()
                .HasMany(f => f.Users)
                .WithOne(u => u.Family)
                .HasForeignKey(u => u.FamilyId)
                // when a Family is deleted, delete its Users as well
                .OnDelete(DeleteBehavior.Cascade);

            // Configure one-to-one: User -> Credentials (FK is on User.CredentialsId)
            modelBuilder.Entity<UserEntity>()
                .HasOne(u => u.Credentials)
                .WithOne()
                .HasForeignKey<UserEntity>(u => u.CredentialsId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure one-to-one: User -> Settings (FK is on User.SettingsId)
            modelBuilder.Entity<UserEntity>()
                .HasOne(u => u.Settings)
                .WithOne()
                .HasForeignKey<UserEntity>(u => u.SettingsId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
