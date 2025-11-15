using Microsoft.EntityFrameworkCore;
using PocViseu.Model.Auth;
using PocViseu.Model.Bussines;
using PocViseu.Model.Config;

namespace PocViseu.Infrastructure.Database
{
    public class WebControlDbContext : DbContext
    {
        public WebControlDbContext()
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            this.ChangeTracker.LazyLoadingEnabled = false;
        }
        public WebControlDbContext(DbContextOptions<WebControlDbContext> options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            this.ChangeTracker.LazyLoadingEnabled = false;

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Global turn off delete behaviour on foreign keys
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }


        public DbSet<User>? Users { get; set; }
        public DbSet<UserProfile>? UserProfile { get; set; }
        public DbSet<WebcorpConfig>? WebcorpConfig { get; set; }
        public DbSet<Process>? Process { get; set; }
        public DbSet<ProcessAttachments>? ProcessAttachments { get; set; }
        public DbSet<ProcessAttachmentsFile>? ProcessAttachmentsFile { get; set; }
        public DbSet<LogSystem> LogSystem { get; set; }
        public DbSet<Prague> Prague { get; set; }
        public DbSet<Culture> Culture { get; set; }
        public DbSet<ApplicationType> ApplicationType { get; set; }

    }
}
