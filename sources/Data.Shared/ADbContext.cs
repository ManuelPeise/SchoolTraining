using Data.Entities;
using Data.Entities.Learning;
using Microsoft.EntityFrameworkCore;

namespace Data.Shared
{
    public abstract class ADbContext: DbContext
    {
        protected ADbContext(DbContextOptions options): base(options) { }

        /// <summary>
        /// Gets or sets the collection of families in the database.
        /// </summary>
        public DbSet<FamilyEntity> FamilyTable { get; set; }
        /// <summary>
        /// Gets or sets the table of users in the database.
        /// </summary>
        public DbSet<UserEntity> UserTable { get; set; }
        /// <summary>
        /// Gets or sets the database table for user credentials.
        /// </summary>
        public DbSet<UserCredentialsEntity> CredentialsTable { get; set; }
        /// <summary>
        /// Gets or sets the database table for user settings entities.
        /// </summary>
        public DbSet<UserSettingsEntity> SettingsTable { get; set; }
        /// <summary>
        /// Gets or sets the table of log message entities for querying and saving log data.
        /// </summary>
        public DbSet<LogMessageEntity> LogMessageTable { get; set; }
        /// <summary>
        /// Gets or sets the table of import file entities for querying and saving import file data.
        /// </summary>
        public DbSet<ImportFileEntity> ImportFileTable { get; set; }

        //***** LEARNING ENTITIES *****//

        /// <summary>
        /// Gets or sets the collection of vocabulary entities in the database.
        /// </summary>
        /// <remarks>This property provides access to the vocabulary records for querying, adding,
        /// updating, or deleting entries using Entity Framework Core. Changes made to this collection are tracked by
        /// the context and persisted to the database when SaveChanges is called.</remarks>
        public DbSet<VocabularyEntity> VocabularyTable { get; set; }
        /// <summary>
        /// Gets or sets the database table for module entities.
        /// </summary>
        public DbSet<ModuleEntity> ModuleTable { get; set; }
        /// <summary>
        /// Gets or sets the table of user module entities for querying and saving instances of <see
        /// cref="UserModuleEntity"/>.
        /// </summary>
        /// <remarks>This property provides access to the user modules within the database context. Use
        /// LINQ queries to retrieve or manipulate user module data.</remarks>
        public DbSet<UserModuleEntity> UserModuleTable { get; set; }
        /// <summary>
        /// Gets or sets the table of sub-module entities for querying and saving instances of <see
        /// cref="SubModuleEntity"/>.
        /// </summary>
        public DbSet<SubModuleEntity> SubModuleTable { get; set; }
        /// <summary>
        /// Gets or sets the collection of units in the database.
        /// </summary>
        public DbSet<UnitEntity> UnitTable { get; set; }

        /// <summary>
        /// Gets or sets the table of unit result entities for querying and saving results in the database.
        /// </summary>
        public DbSet<UnitResultEntity> UnitResultTable { get; set; }
        /// <summary>
        /// Gets or sets the database table for vocabulary unit entities.
        /// </summary>
        public DbSet<VocabularyUnitEntity> VocabularyUnitTable { get; set; }


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

            modelBuilder.Entity<ModuleEntity>()
                .HasMany(m => m.SubModules)
                .WithOne(sm => sm.Module)
                .HasForeignKey(sm => sm.ModuleId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SubModuleEntity>()
                .HasMany(sm => sm.Units)
                .WithOne(u => u.SubModule)
                .HasForeignKey(u => u.SubModuleId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserModuleEntity>()
                .HasOne(um => um.User)
                .WithMany(u => u.UserModules)
                .HasForeignKey(um => um.UserId);

            modelBuilder.Entity<UserModuleEntity>()
                .HasOne(um => um.Module)
                .WithMany()
                .HasForeignKey(um => um.ModuleId);

            modelBuilder.Entity<UnitResultEntity>()
                .HasOne(ur => ur.User)
                .WithMany()
                .HasForeignKey(ur => ur.UserId);

            modelBuilder.Entity<UnitResultEntity>()
                .HasOne(ur => ur.Unit)
                .WithMany()
                .HasForeignKey(ur => ur.UnitId);

            // Configure many-to-many: Unit <-> Vocabulary via VocabularyUnitEntity
            modelBuilder.Entity<VocabularyUnitEntity>()
                .HasKey(vu => new { vu.UnitId, vu.VocabularyId });

            modelBuilder.Entity<VocabularyUnitEntity>()
                .HasOne(vu => vu.Unit)
                .WithMany()
                .HasForeignKey(vu => vu.UnitId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<VocabularyUnitEntity>()
                .HasOne(vu => vu.Vocabulary)
                .WithMany()
                .HasForeignKey(vu => vu.VocabularyId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
