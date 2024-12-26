﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using online_is_bulma_platformu.Data;

#nullable disable

namespace online_is_bulma_platformu.Migrations
{
    [DbContext(typeof(JobPortalContext))]
    [Migration("20241226111829_AddJobApplicationsRelation")]
    partial class AddJobApplicationsRelation
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("online_is_bulma_platformu.Models.JobApplication", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("ApplicationDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("JobListingId")
                        .HasColumnType("int");

                    b.Property<int>("JobSeekerId")
                        .HasColumnType("int");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("JobListingId");

                    b.HasIndex("JobSeekerId");

                    b.ToTable("JobApplications");
                });

            modelBuilder.Entity("online_is_bulma_platformu.Models.JobListing", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Deadline")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("EmployerId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("EmployerId");

                    b.ToTable("JobListings");
                });

            modelBuilder.Entity("online_is_bulma_platformu.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("online_is_bulma_platformu.Models.UserMessage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("JobListingId")
                        .HasColumnType("int");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ReceiverId")
                        .HasColumnType("int");

                    b.Property<int>("SenderId")
                        .HasColumnType("int");

                    b.Property<DateTime>("SentAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("JobListingId");

                    b.HasIndex("ReceiverId");

                    b.HasIndex("SenderId");

                    b.ToTable("UserMessages");
                });

            modelBuilder.Entity("online_is_bulma_platformu.Models.JobApplication", b =>
                {
                    b.HasOne("online_is_bulma_platformu.Models.JobListing", "JobListing")
                        .WithMany()
                        .HasForeignKey("JobListingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("online_is_bulma_platformu.Models.User", "JobSeeker")
                        .WithMany()
                        .HasForeignKey("JobSeekerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("JobListing");

                    b.Navigation("JobSeeker");
                });

            modelBuilder.Entity("online_is_bulma_platformu.Models.JobListing", b =>
                {
                    b.HasOne("online_is_bulma_platformu.Models.User", "Employer")
                        .WithMany()
                        .HasForeignKey("EmployerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employer");
                });

            modelBuilder.Entity("online_is_bulma_platformu.Models.UserMessage", b =>
                {
                    b.HasOne("online_is_bulma_platformu.Models.JobListing", "JobListing")
                        .WithMany()
                        .HasForeignKey("JobListingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("online_is_bulma_platformu.Models.User", "Receiver")
                        .WithMany()
                        .HasForeignKey("ReceiverId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("online_is_bulma_platformu.Models.User", "Sender")
                        .WithMany()
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("JobListing");

                    b.Navigation("Receiver");

                    b.Navigation("Sender");
                });
#pragma warning restore 612, 618
        }
    }
}