﻿// <auto-generated />
using System;
using KsiazeczkaPttk.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace KsiazeczkaPTTK.DAL.Migrations
{
    [DbContext(typeof(TouristsBookContext))]
    partial class TouristsBookContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("KsiazeczkaPttk.Domain.Models.Confirmation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsAdministration")
                        .HasColumnType("boolean");

                    b.Property<int>("TerrainPointId")
                        .HasColumnType("integer");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.HasKey("Id");

                    b.HasIndex("TerrainPointId");

                    b.ToTable("Confirmations");
                });

            modelBuilder.Entity("KsiazeczkaPttk.Domain.Models.GotPttk", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Level")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id");

                    b.HasIndex("Level")
                        .IsUnique();

                    b.ToTable("GotPttk");
                });

            modelBuilder.Entity("KsiazeczkaPttk.Domain.Models.GotPttkOwnership", b =>
                {
                    b.Property<string>("Owner")
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.Property<int>("GotPttkId")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("DateOfAward")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Owner", "GotPttkId");

                    b.HasIndex("GotPttkId");

                    b.ToTable("GotPttkOwnerships");
                });

            modelBuilder.Entity("KsiazeczkaPttk.Domain.Models.MountainGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("MountainGroups");
                });

            modelBuilder.Entity("KsiazeczkaPttk.Domain.Models.MountainRange", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("GroupId")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("MountainRanges");
                });

            modelBuilder.Entity("KsiazeczkaPttk.Domain.Models.Segment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("FromId")
                        .HasColumnType("integer");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<int>("MountainRangeId")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<int>("Points")
                        .HasColumnType("integer");

                    b.Property<int>("PointsBack")
                        .HasColumnType("integer");

                    b.Property<int>("TargetId")
                        .HasColumnType("integer");

                    b.Property<string>("TouristsBookOwner")
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.Property<int>("Version")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("FromId");

                    b.HasIndex("MountainRangeId");

                    b.HasIndex("TargetId");

                    b.HasIndex("TouristsBookOwner");

                    b.ToTable("Segments");
                });

            modelBuilder.Entity("KsiazeczkaPttk.Domain.Models.SegmentClose", b =>
                {
                    b.Property<int>("SegmentId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("ClosedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Cause")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<DateTime?>("OpenedDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("SegmentId", "ClosedDate");

                    b.ToTable("SegmentCloses");
                });

            modelBuilder.Entity("KsiazeczkaPttk.Domain.Models.SegmentConfirmation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("ConfirmationId")
                        .HasColumnType("integer");

                    b.Property<int>("SegmentTravelId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ConfirmationId");

                    b.HasIndex("SegmentTravelId");

                    b.ToTable("SegmentConfirmations");
                });

            modelBuilder.Entity("KsiazeczkaPttk.Domain.Models.SegmentTravel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<bool>("IsBack")
                        .HasColumnType("boolean");

                    b.Property<int>("Order")
                        .HasColumnType("integer");

                    b.Property<int>("SegmentId")
                        .HasColumnType("integer");

                    b.Property<int>("TripId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("SegmentId");

                    b.HasIndex("TripId");

                    b.ToTable("SegmentTravels");
                });

            modelBuilder.Entity("KsiazeczkaPttk.Domain.Models.TerrainPoint", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<double>("Lat")
                        .HasColumnType("double precision");

                    b.Property<double>("Lng")
                        .HasColumnType("double precision");

                    b.Property<double>("Mnpm")
                        .HasColumnType("double precision");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("TouristsBookOwner")
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("TouristsBookOwner");

                    b.ToTable("TerrainPoints");
                });

            modelBuilder.Entity("KsiazeczkaPttk.Domain.Models.TouristsBook", b =>
                {
                    b.Property<string>("OwnerId")
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.Property<bool>("Disability")
                        .HasColumnType("boolean");

                    b.Property<int>("Points")
                        .HasColumnType("integer");

                    b.HasKey("OwnerId");

                    b.ToTable("TouristsBooks");
                });

            modelBuilder.Entity("KsiazeczkaPttk.Domain.Models.Trip", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<string>("TouristsBookId")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.HasKey("Id");

                    b.HasIndex("TouristsBookId");

                    b.ToTable("Trips");
                });

            modelBuilder.Entity("KsiazeczkaPttk.Domain.Models.User", b =>
                {
                    b.Property<string>("Login")
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(160)
                        .HasColumnType("character varying(160)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(160)
                        .HasColumnType("character varying(160)");

                    b.Property<string>("UserRoleName")
                        .IsRequired()
                        .HasColumnType("character varying(40)");

                    b.HasKey("Login");

                    b.HasIndex("UserRoleName");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("KsiazeczkaPttk.Domain.Models.UserRole", b =>
                {
                    b.Property<string>("Name")
                        .HasMaxLength(40)
                        .HasColumnType("character varying(40)");

                    b.HasKey("Name");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("KsiazeczkaPttk.Domain.Models.Confirmation", b =>
                {
                    b.HasOne("KsiazeczkaPttk.Domain.Models.TerrainPoint", "TerrainPoint")
                        .WithMany()
                        .HasForeignKey("TerrainPointId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TerrainPoint");
                });

            modelBuilder.Entity("KsiazeczkaPttk.Domain.Models.GotPttkOwnership", b =>
                {
                    b.HasOne("KsiazeczkaPttk.Domain.Models.GotPttk", "GotPttk")
                        .WithMany()
                        .HasForeignKey("GotPttkId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("KsiazeczkaPttk.Domain.Models.TouristsBook", "TouristsBook")
                        .WithMany()
                        .HasForeignKey("Owner")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("GotPttk");

                    b.Navigation("TouristsBook");
                });

            modelBuilder.Entity("KsiazeczkaPttk.Domain.Models.MountainRange", b =>
                {
                    b.HasOne("KsiazeczkaPttk.Domain.Models.MountainGroup", "MountainGroup")
                        .WithMany()
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MountainGroup");
                });

            modelBuilder.Entity("KsiazeczkaPttk.Domain.Models.Segment", b =>
                {
                    b.HasOne("KsiazeczkaPttk.Domain.Models.TerrainPoint", "From")
                        .WithMany()
                        .HasForeignKey("FromId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("KsiazeczkaPttk.Domain.Models.MountainRange", "MountainRange")
                        .WithMany()
                        .HasForeignKey("MountainRangeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("KsiazeczkaPttk.Domain.Models.TerrainPoint", "Target")
                        .WithMany()
                        .HasForeignKey("TargetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("KsiazeczkaPttk.Domain.Models.TouristsBook", "TouristsBook")
                        .WithMany()
                        .HasForeignKey("TouristsBookOwner");

                    b.Navigation("From");

                    b.Navigation("MountainRange");

                    b.Navigation("Target");

                    b.Navigation("TouristsBook");
                });

            modelBuilder.Entity("KsiazeczkaPttk.Domain.Models.SegmentClose", b =>
                {
                    b.HasOne("KsiazeczkaPttk.Domain.Models.Segment", "Segment")
                        .WithMany()
                        .HasForeignKey("SegmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Segment");
                });

            modelBuilder.Entity("KsiazeczkaPttk.Domain.Models.SegmentConfirmation", b =>
                {
                    b.HasOne("KsiazeczkaPttk.Domain.Models.Confirmation", "Confirmation")
                        .WithMany()
                        .HasForeignKey("ConfirmationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("KsiazeczkaPttk.Domain.Models.SegmentTravel", "SegmentTravel")
                        .WithMany()
                        .HasForeignKey("SegmentTravelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Confirmation");

                    b.Navigation("SegmentTravel");
                });

            modelBuilder.Entity("KsiazeczkaPttk.Domain.Models.SegmentTravel", b =>
                {
                    b.HasOne("KsiazeczkaPttk.Domain.Models.Segment", "Segment")
                        .WithMany()
                        .HasForeignKey("SegmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("KsiazeczkaPttk.Domain.Models.Trip", "Trip")
                        .WithMany("Segments")
                        .HasForeignKey("TripId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Segment");

                    b.Navigation("Trip");
                });

            modelBuilder.Entity("KsiazeczkaPttk.Domain.Models.TerrainPoint", b =>
                {
                    b.HasOne("KsiazeczkaPttk.Domain.Models.TouristsBook", "TouristsBook")
                        .WithMany()
                        .HasForeignKey("TouristsBookOwner");

                    b.Navigation("TouristsBook");
                });

            modelBuilder.Entity("KsiazeczkaPttk.Domain.Models.TouristsBook", b =>
                {
                    b.HasOne("KsiazeczkaPttk.Domain.Models.User", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("KsiazeczkaPttk.Domain.Models.Trip", b =>
                {
                    b.HasOne("KsiazeczkaPttk.Domain.Models.TouristsBook", "TouristsBook")
                        .WithMany()
                        .HasForeignKey("TouristsBookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TouristsBook");
                });

            modelBuilder.Entity("KsiazeczkaPttk.Domain.Models.User", b =>
                {
                    b.HasOne("KsiazeczkaPttk.Domain.Models.UserRole", "UserRole")
                        .WithMany()
                        .HasForeignKey("UserRoleName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserRole");
                });

            modelBuilder.Entity("KsiazeczkaPttk.Domain.Models.Trip", b =>
                {
                    b.Navigation("Segments");
                });
#pragma warning restore 612, 618
        }
    }
}
