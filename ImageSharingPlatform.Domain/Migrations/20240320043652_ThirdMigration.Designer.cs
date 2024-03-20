﻿// <auto-generated />
using System;
using ImageSharingPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ImageSharingPlatform.Domain.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240320043652_ThirdMigration")]
    partial class ThirdMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ImageSharingPlatform.Domain.Entities.ImageCategory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("category_name");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("description");

                    b.HasKey("Id");

                    b.ToTable("image_category", (string)null);
                });

            modelBuilder.Entity("ImageSharingPlatform.Domain.Entities.ImageRequest", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ArtistId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime2")
                        .HasColumnName("created_at");

                    b.Property<string>("Description")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)")
                        .HasColumnName("description");

                    b.Property<DateTime>("ExpectedTime")
                        .HasColumnType("datetime2")
                        .HasColumnName("expected_time");

                    b.Property<byte[]>("ImageBlob")
                        .HasColumnType("varbinary(max)")
                        .HasColumnName("image");

                    b.Property<double>("Price")
                        .HasColumnType("float")
                        .HasColumnName("price");

                    b.Property<int>("RequestStatus")
                        .HasColumnType("int")
                        .HasColumnName("status");

                    b.Property<Guid?>("RequesterUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("title");

                    b.HasKey("Id");

                    b.HasIndex("ArtistId");

                    b.HasIndex("RequesterUserId");

                    b.ToTable("image_request", (string)null);
                });

            modelBuilder.Entity("ImageSharingPlatform.Domain.Entities.OwnedSubscription", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("PurchasedTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("SubscriptionPackageId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("SubscriptionPackageId");

                    b.HasIndex("UserId");

                    b.ToTable("owned_subscription", (string)null);
                });

            modelBuilder.Entity("ImageSharingPlatform.Domain.Entities.RequestDetail", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Comment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ExpectedTime")
                        .HasColumnType("datetime2")
                        .HasColumnName("expected_time");

                    b.Property<double?>("NewPrice")
                        .HasColumnType("float");

                    b.Property<Guid>("RequestId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("RequestId");

                    b.HasIndex("UserId");

                    b.ToTable("RequestDetails");
                });

            modelBuilder.Entity("ImageSharingPlatform.Domain.Entities.Review", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Comment")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("comment");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("created_at");

                    b.Property<int>("Rating")
                        .HasColumnType("int")
                        .HasColumnName("rating");

                    b.Property<Guid?>("SharedImageId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("SharedImageId");

                    b.HasIndex("UserId");

                    b.ToTable("review", (string)null);
                });

            modelBuilder.Entity("ImageSharingPlatform.Domain.Entities.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("UserRole")
                        .HasColumnType("int")
                        .HasColumnName("role");

                    b.HasKey("Id");

                    b.ToTable("role", (string)null);
                });

            modelBuilder.Entity("ImageSharingPlatform.Domain.Entities.SharedImage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ArtistId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("created_at");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("description");

                    b.Property<Guid?>("ImageCategoryId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ImageName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("image_name");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("image_url");

                    b.Property<bool>("IsPremium")
                        .HasColumnType("bit")
                        .HasColumnName("is_premium");

                    b.HasKey("Id");

                    b.HasIndex("ArtistId");

                    b.HasIndex("ImageCategoryId");

                    b.ToTable("shared_image", (string)null);
                });

            modelBuilder.Entity("ImageSharingPlatform.Domain.Entities.SubscriptionPackage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ArtistId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("ArtistId")
                        .IsUnique()
                        .HasFilter("[ArtistId] IS NOT NULL");

                    b.ToTable("subscription_package", (string)null);
                });

            modelBuilder.Entity("ImageSharingPlatform.Domain.Entities.Transaction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("Amount")
                        .HasColumnType("float");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("TransactionDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("TransactionType")
                        .HasColumnType("int");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("ImageSharingPlatform.Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AvatarUrl")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)")
                        .HasColumnName("avatar_url");

                    b.Property<long>("Balance")
                        .HasColumnType("bigint")
                        .HasColumnName("balance");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("email");

                    b.Property<string>("FirstName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("first_name");

                    b.Property<string>("LastName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("last_name");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("password");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("username");

                    b.HasKey("Id");

                    b.ToTable("user", (string)null);
                });

            modelBuilder.Entity("UserRole", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRole");
                });

            modelBuilder.Entity("ImageSharingPlatform.Domain.Entities.ImageRequest", b =>
                {
                    b.HasOne("ImageSharingPlatform.Domain.Entities.User", "Artist")
                        .WithMany()
                        .HasForeignKey("ArtistId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("ImageSharingPlatform.Domain.Entities.User", "RequesterUser")
                        .WithMany()
                        .HasForeignKey("RequesterUserId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Artist");

                    b.Navigation("RequesterUser");
                });

            modelBuilder.Entity("ImageSharingPlatform.Domain.Entities.OwnedSubscription", b =>
                {
                    b.HasOne("ImageSharingPlatform.Domain.Entities.SubscriptionPackage", "SubscriptionPackage")
                        .WithMany()
                        .HasForeignKey("SubscriptionPackageId");

                    b.HasOne("ImageSharingPlatform.Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("SubscriptionPackage");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ImageSharingPlatform.Domain.Entities.RequestDetail", b =>
                {
                    b.HasOne("ImageSharingPlatform.Domain.Entities.ImageRequest", "Request")
                        .WithMany()
                        .HasForeignKey("RequestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ImageSharingPlatform.Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("Request");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ImageSharingPlatform.Domain.Entities.Review", b =>
                {
                    b.HasOne("ImageSharingPlatform.Domain.Entities.SharedImage", "SharedImage")
                        .WithMany("Reviews")
                        .HasForeignKey("SharedImageId");

                    b.HasOne("ImageSharingPlatform.Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("SharedImage");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ImageSharingPlatform.Domain.Entities.SharedImage", b =>
                {
                    b.HasOne("ImageSharingPlatform.Domain.Entities.User", "Artist")
                        .WithMany()
                        .HasForeignKey("ArtistId");

                    b.HasOne("ImageSharingPlatform.Domain.Entities.ImageCategory", "ImageCategory")
                        .WithMany()
                        .HasForeignKey("ImageCategoryId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Artist");

                    b.Navigation("ImageCategory");
                });

            modelBuilder.Entity("ImageSharingPlatform.Domain.Entities.SubscriptionPackage", b =>
                {
                    b.HasOne("ImageSharingPlatform.Domain.Entities.User", "Artist")
                        .WithOne()
                        .HasForeignKey("ImageSharingPlatform.Domain.Entities.SubscriptionPackage", "ArtistId");

                    b.Navigation("Artist");
                });

            modelBuilder.Entity("ImageSharingPlatform.Domain.Entities.Transaction", b =>
                {
                    b.HasOne("ImageSharingPlatform.Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("UserRole", b =>
                {
                    b.HasOne("ImageSharingPlatform.Domain.Entities.Role", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ImageSharingPlatform.Domain.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ImageSharingPlatform.Domain.Entities.SharedImage", b =>
                {
                    b.Navigation("Reviews");
                });
#pragma warning restore 612, 618
        }
    }
}
