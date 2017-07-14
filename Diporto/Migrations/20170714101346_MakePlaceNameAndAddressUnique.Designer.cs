using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Diporto.Database;

namespace Diporto.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20170714101346_MakePlaceNameAndAddressUnique")]
    partial class MakePlaceNameAndAddressUnique
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "1.1.2");

            modelBuilder.Entity("Diporto.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("category");
                });

            modelBuilder.Entity("Diporto.Models.Place", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnName("address");

                    b.Property<float>("Lat")
                        .HasColumnName("lat");

                    b.Property<float>("Lon")
                        .HasColumnName("lon");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("name");

                    b.Property<string>("OpeningHours")
                        .IsRequired()
                        .HasColumnName("opening_hours");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnName("phone");

                    b.HasKey("Id");

                    b.HasIndex("Name", "Address")
                        .IsUnique();

                    b.ToTable("place");
                });

            modelBuilder.Entity("Diporto.Models.PlaceCategory", b =>
                {
                    b.Property<int>("PlaceId")
                        .HasColumnName("place_id");

                    b.Property<int>("CategoryId")
                        .HasColumnName("category_id");

                    b.Property<int>("Id")
                        .HasColumnName("id");

                    b.HasKey("PlaceId", "CategoryId");

                    b.HasIndex("CategoryId");

                    b.ToTable("place_category");
                });

            modelBuilder.Entity("Diporto.Models.PlacePhoto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<string>("URL")
                        .IsRequired()
                        .HasColumnName("url");

                    b.Property<int?>("place_id")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("place_id");

                    b.ToTable("place_photo");
                });

            modelBuilder.Entity("Diporto.Models.PlaceReview", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<string>("AuthorName")
                        .IsRequired()
                        .HasColumnName("author_name");

                    b.Property<string>("AuthorProfileImageURL")
                        .HasColumnName("author_profile_image_url");

                    b.Property<float>("Rating")
                        .HasColumnName("rating");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnName("text");

                    b.Property<DateTime>("Time")
                        .HasColumnName("time");

                    b.Property<int?>("place_id")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("place_id");

                    b.ToTable("place_review");
                });

            modelBuilder.Entity("Diporto.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<bool>("IsAdmin")
                        .HasColumnName("is_admin");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("name");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnName("password");

                    b.HasKey("Id");

                    b.ToTable("user");
                });

            modelBuilder.Entity("Diporto.Models.PlaceCategory", b =>
                {
                    b.HasOne("Diporto.Models.Category", "Category")
                        .WithMany("PlaceCategories")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Diporto.Models.Place", "Place")
                        .WithMany("PlaceCategories")
                        .HasForeignKey("PlaceId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Diporto.Models.PlacePhoto", b =>
                {
                    b.HasOne("Diporto.Models.Place", "Place")
                        .WithMany("PlacePhotos")
                        .HasForeignKey("place_id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Diporto.Models.PlaceReview", b =>
                {
                    b.HasOne("Diporto.Models.Place", "Place")
                        .WithMany("PlaceReviews")
                        .HasForeignKey("place_id")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
