﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RazorPagesMovie.Data;

#nullable disable

namespace RazorPagesMovie.Migrations
{
    [DbContext(typeof(RazorPagesMovieContext))]
    [Migration("20241216215338_AddDirectorTable")]
    partial class AddDirectorTable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("RazorPagesMovie.Models.Director", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.HasKey("Id");

                    b.ToTable("Directors");
                });

            modelBuilder.Entity("RazorPagesMovie.Models.Movie", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Genre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Rating")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ReleaseDate")
                        .HasColumnType("datetime2");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Movie");
                });

            modelBuilder.Entity("RazorPagesMovie.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Password = "password",
                            Role = 2,
                            Timestamp = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
                            Username = "admin"
                        },
                        new
                        {
                            Id = 2,
                            Password = "password",
                            Role = 2,
                            Timestamp = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
                            Username = "admin1"
                        },
                        new
                        {
                            Id = 3,
                            Password = "password",
                            Role = 2,
                            Timestamp = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
                            Username = "admin2"
                        },
                        new
                        {
                            Id = 4,
                            Password = "password",
                            Role = 2,
                            Timestamp = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
                            Username = "admin3"
                        },
                        new
                        {
                            Id = 5,
                            Password = "password",
                            Role = 2,
                            Timestamp = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
                            Username = "admin4"
                        },
                        new
                        {
                            Id = 6,
                            Password = "password",
                            Role = 2,
                            Timestamp = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
                            Username = "admin5"
                        },
                        new
                        {
                            Id = 7,
                            Password = "password",
                            Role = 2,
                            Timestamp = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
                            Username = "admin6"
                        },
                        new
                        {
                            Id = 8,
                            Password = "password",
                            Role = 2,
                            Timestamp = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
                            Username = "admin7"
                        },
                        new
                        {
                            Id = 9,
                            Password = "password",
                            Role = 2,
                            Timestamp = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
                            Username = "admin8"
                        },
                        new
                        {
                            Id = 10,
                            Password = "password",
                            Role = 2,
                            Timestamp = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
                            Username = "admin9"
                        },
                        new
                        {
                            Id = 11,
                            Password = "password",
                            Role = 0,
                            Timestamp = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
                            Username = "user"
                        },
                        new
                        {
                            Id = 12,
                            Password = "password",
                            Role = 0,
                            Timestamp = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
                            Username = "user1"
                        },
                        new
                        {
                            Id = 13,
                            Password = "password",
                            Role = 0,
                            Timestamp = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
                            Username = "user2"
                        },
                        new
                        {
                            Id = 14,
                            Password = "password",
                            Role = 0,
                            Timestamp = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
                            Username = "user3"
                        },
                        new
                        {
                            Id = 15,
                            Password = "password",
                            Role = 0,
                            Timestamp = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
                            Username = "user4"
                        },
                        new
                        {
                            Id = 16,
                            Password = "password",
                            Role = 0,
                            Timestamp = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
                            Username = "user5"
                        },
                        new
                        {
                            Id = 17,
                            Password = "password",
                            Role = 0,
                            Timestamp = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
                            Username = "user6"
                        },
                        new
                        {
                            Id = 18,
                            Password = "password",
                            Role = 0,
                            Timestamp = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
                            Username = "user7"
                        },
                        new
                        {
                            Id = 19,
                            Password = "password",
                            Role = 0,
                            Timestamp = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
                            Username = "user8"
                        },
                        new
                        {
                            Id = 20,
                            Password = "password",
                            Role = 0,
                            Timestamp = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
                            Username = "user9"
                        });
                });

            modelBuilder.Entity("RazorPagesMovie.Models.Movie", b =>
                {
                    b.HasOne("RazorPagesMovie.Models.User", "User")
                        .WithMany("Movies")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("User");
                });

            modelBuilder.Entity("RazorPagesMovie.Models.User", b =>
                {
                    b.Navigation("Movies");
                });
#pragma warning restore 612, 618
        }
    }
}
