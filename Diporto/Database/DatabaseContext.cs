using Microsoft.EntityFrameworkCore;
using Diporto.Models;
using System.Linq;

namespace Diporto.Database {
  public class DatabaseContext : DbContext {
    public DbSet<User> Users { get; set; }
    public DbSet<Place> Places { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
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
    }
  }
}