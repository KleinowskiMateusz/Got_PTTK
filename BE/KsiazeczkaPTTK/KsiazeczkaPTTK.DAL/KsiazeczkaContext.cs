using KsiazeczkaPttk.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace KsiazeczkaPttk.DAL
{
    public class KsiazeczkaContext : DbContext
    {
        public KsiazeczkaContext(DbContextOptions<KsiazeczkaContext> options) : base(options) {}

        public async Task Migrate()
        {
            await Database.MigrateAsync();
        }

        public DbSet<GotPttk> GotPttk { get; set; }
        public DbSet<GotPttkOwnership> GotPttkOwnerships { get; set; }
        public DbSet<TouristsBook> TouristsBooks { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<TerrainPoint> TerrainPoints { get; set; }
        public DbSet<MountainGroup> MountainGroups { get; set; }
        public DbSet<MountainRange> MountainRanges { get; set; }
        public DbSet<Segment> Segments { get; set; }
        public DbSet<SegmentClose> SegmentCloses { get; set; }
        public DbSet<Confirmation> Confirmations { get; set; }
        public DbSet<SegmentTravel> SegmentTravels { get; set; }
        public DbSet<SegmentConfirmation> SegmentConfirmations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GotPttk>()
                .HasIndex(x => x.Level)
                .IsUnique(true);

            modelBuilder.Entity<GotPttkOwnership>()
                .HasKey(x => new { x.Owner, x.GotPttkId });

            modelBuilder.Entity<TerrainPoint>()
                .HasIndex(x => x.Name)
                .IsUnique(true);

            modelBuilder.Entity<MountainGroup>()
                .HasIndex(x => x.Name)
                .IsUnique(true);

            modelBuilder.Entity<MountainRange>()
                .HasIndex(x => x.Name)
                .IsUnique(true);

            modelBuilder.Entity<SegmentClose>()
               .HasKey(x => new { x.SegmentId, x.ClosedDate });

            base.OnModelCreating(modelBuilder);
        }
    }
}
