using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StreamingDB.Classes;

namespace StreamingDB.Context
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Track> Tracks { get; set; } = null!;
        public DbSet<Album> Albums { get; set; } = null!;
        public DbSet<Artist> Artists { get; set; } = null!;
        public DbSet<TopSong> Chart { get; set; } = null!;

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            //Database.EnsureDeleted();
            //Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Track>(TrackConfigure);
            modelBuilder.Entity<Artist>(ArtistConfigure);
            modelBuilder.Entity<Album>(AlbumConfigure);
        }

        public void TrackConfigure(EntityTypeBuilder<Track> builder)
        {
            builder.HasIndex(t => t.Id).HasName("IX_Track_Id").IsUnique();
            builder.HasIndex(t => t.Name).HasName("IX_Track_Name");
            builder.Property(t => t.Plays).HasDefaultValue(0);
        }

        public void ArtistConfigure(EntityTypeBuilder<Artist> builder)
        {
            builder.HasIndex(a => a.Id).HasName("IX_Artist_Id").IsUnique();
            builder.HasIndex(a => a.Name).HasName("IX_Artist_Name");
            builder.Property(a => a.Plays).HasDefaultValue(0);
        }

        public void AlbumConfigure(EntityTypeBuilder<Album> builder)
        {
            builder.HasIndex(a => a.Id).HasName("IX_Album_Id").IsUnique();
            builder.HasIndex(a => a.Name).HasName("IX_Album_Name");
            builder.Property(a => a.Plays).HasDefaultValue(0);
        }
    }
}
