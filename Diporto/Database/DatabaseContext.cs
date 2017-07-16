using Microsoft.EntityFrameworkCore;
using Diporto.Models;
using System.Linq;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;

namespace Diporto.Database {
  public class DatabaseContext : IdentityDbContext<User, IdentityRole<int>, int> {
    public DbSet<Place> Places { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<PlaceReview> PlaceReviews { get; set; }
    public DbSet<PlacePhoto> PlacePhotos { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      base.OnModelCreating(modelBuilder);

      modelBuilder.HasPostgresExtension("cube");
      modelBuilder.HasPostgresExtension("earthdistance");
      modelBuilder.HasPostgresExtension("postgis");

      modelBuilder.Entity<PlaceCategory>()
        .HasKey(pc => new { pc.PlaceId, pc.CategoryId });
      
      modelBuilder.Entity<PlaceCategory>()
        .HasOne(pc => pc.Place)
        .WithMany(place => place.PlaceCategories)
        .HasForeignKey(pc => pc.PlaceId);

      modelBuilder.Entity<PlaceCategory>()
        .HasOne(pc => pc.Category)
        .WithMany(cat => cat.PlaceCategories)
        .HasForeignKey(pc => pc.CategoryId);

      modelBuilder.Entity<Place>()
        .HasMany(place => place.PlacePhotos)
        .WithOne(photo => photo.Place)
        .HasForeignKey("place_id");

      modelBuilder.Entity<Place>()
        .HasMany(place => place.PlaceReviews)
        .WithOne(review => review.Place)
        .HasForeignKey("place_id");

      modelBuilder.Entity<Category>()
        .HasIndex(cat => cat.Name)
        .IsUnique();

      modelBuilder.Entity<Place>()
        .HasIndex("Name", "Address")
        .IsUnique();

      modelBuilder.Entity<User>()
        .HasIndex(user => user.UserName)
        .IsUnique();

      modelBuilder.Entity<User>(entity => {
        entity.ToTable("user");
        entity.Property(user => user.Id).HasColumnName("id");
        entity.Property(user => user.UserName).HasColumnName("user_name");
        entity.Property(user => user.Email).HasColumnName("email");
        entity.Property(user => user.PasswordHash).HasColumnName("password_hash");
        entity.Property(user => user.TwoFactorEnabled).HasColumnName("is_two_factor_enabled");
        entity.Property(user => user.EmailConfirmed).HasColumnName("email_confirmed");
        entity.Property(user => user.AccessFailedCount).HasColumnName("access_failed_count");
      });
    }
  }
}
