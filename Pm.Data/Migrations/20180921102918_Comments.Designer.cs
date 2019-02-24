﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Pm.Data;

namespace Pm.Data.Migrations
{
    [DbContext(typeof(PmEntities))]
    [Migration("20180921102918_Comments")]
    partial class Comments
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Pm.Data.Entity.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("Name");

                    b.Property<int?>("PostId");

                    b.Property<string>("Text");

                    b.HasKey("Id");

                    b.HasIndex("PostId");

                    b.ToTable("Comment");
                });

            modelBuilder.Entity("Pm.Data.Entity.Image", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<byte[]>("Data");

                    b.Property<string>("Mime");

                    b.HasKey("Id");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("Pm.Data.Entity.Post", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("AuthorId");

                    b.Property<string>("Body");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<int>("Downvotes");

                    b.Property<bool>("Published");

                    b.Property<string>("Subtitle");

                    b.Property<int?>("ThumbnailImageId");

                    b.Property<string>("Title");

                    b.Property<DateTime?>("UpdatedAt");

                    b.Property<int>("Upvotes");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("ThumbnailImageId")
                        .IsUnique();

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("Pm.Data.Entity.PostTag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("PostId");

                    b.Property<string>("Tag");

                    b.HasKey("Id");

                    b.HasIndex("PostId");

                    b.HasIndex("Tag");

                    b.ToTable("PostTags");
                });

            modelBuilder.Entity("Pm.Data.Entity.Rating", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Direction");

                    b.Property<int>("PostId");

                    b.Property<string>("Token");

                    b.HasKey("Id");

                    b.HasIndex("PostId", "Token")
                        .IsUnique();

                    b.ToTable("Ratings");
                });

            modelBuilder.Entity("Pm.Data.Entity.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("DisplayName")
                        .IsRequired();

                    b.Property<string>("PasswordHash")
                        .IsRequired();

                    b.Property<string>("Username")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Pm.Data.Entity.Comment", b =>
                {
                    b.HasOne("Pm.Data.Entity.Post", "Post")
                        .WithMany("Comments")
                        .HasForeignKey("PostId");
                });

            modelBuilder.Entity("Pm.Data.Entity.Post", b =>
                {
                    b.HasOne("Pm.Data.Entity.User", "Author")
                        .WithMany("Posts")
                        .HasForeignKey("AuthorId");

                    b.HasOne("Pm.Data.Entity.Image", "ThumbnailImage")
                        .WithOne("Post")
                        .HasForeignKey("Pm.Data.Entity.Post", "ThumbnailImageId");
                });

            modelBuilder.Entity("Pm.Data.Entity.PostTag", b =>
                {
                    b.HasOne("Pm.Data.Entity.Post", "Post")
                        .WithMany("Tags")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Pm.Data.Entity.Rating", b =>
                {
                    b.HasOne("Pm.Data.Entity.Post", "Post")
                        .WithMany("Ratings")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}