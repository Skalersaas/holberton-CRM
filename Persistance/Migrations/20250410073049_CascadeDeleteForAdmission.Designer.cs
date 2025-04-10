﻿// <auto-generated />
using System;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Persistance.Data;

#nullable disable

namespace Persistance.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20250410073049_CascadeDeleteForAdmission")]
    partial class CascadeDeleteForAdmission
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Domain.Models.Entities.Admission", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasAnnotation("Relational:JsonPropertyName", "id");

                    b.Property<DateTime>("ApplyDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Program")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Slug")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<Guid>("StudentGuid")
                        .HasColumnType("uuid")
                        .HasAnnotation("Relational:JsonPropertyName", "studentId");

                    b.Property<string>("StudentSlug")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("UserGuid")
                        .HasColumnType("uuid")
                        .HasAnnotation("Relational:JsonPropertyName", "userId");

                    b.Property<string>("UserSlug")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Guid");

                    b.HasIndex("Slug")
                        .IsUnique();

                    b.HasIndex("StudentGuid");

                    b.HasIndex("UserGuid");

                    b.ToTable("Admissions");
                });

            modelBuilder.Entity("Domain.Models.Entities.AdmissionChange", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasAnnotation("Relational:JsonPropertyName", "id");

                    b.Property<Guid>("AdmissionGuid")
                        .HasColumnType("uuid")
                        .HasAnnotation("Relational:JsonPropertyName", "admissionId");

                    b.Property<DateTime>("ChangedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<JsonDocument>("Data")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.Property<string>("Slug")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Guid");

                    b.HasIndex("AdmissionGuid");

                    b.HasIndex("Slug")
                        .IsUnique();

                    b.ToTable("AdmissionChanges");
                });

            modelBuilder.Entity("Domain.Models.Entities.AdmissionNote", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasAnnotation("Relational:JsonPropertyName", "id");

                    b.Property<Guid>("AdmissionGuid")
                        .HasColumnType("uuid")
                        .HasAnnotation("Relational:JsonPropertyName", "admissionId");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Slug")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Guid");

                    b.HasIndex("AdmissionGuid");

                    b.HasIndex("Slug")
                        .IsUnique();

                    b.ToTable("AdmissionNotes");
                });

            modelBuilder.Entity("Domain.Models.Entities.Student", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasAnnotation("Relational:JsonPropertyName", "id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Slug")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Guid");

                    b.HasIndex("Slug")
                        .IsUnique();

                    b.ToTable("Students");
                });

            modelBuilder.Entity("Domain.Models.Entities.User", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasAnnotation("Relational:JsonPropertyName", "id");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Role")
                        .HasColumnType("integer");

                    b.Property<string>("Slug")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Guid");

                    b.HasIndex("Login")
                        .IsUnique();

                    b.HasIndex("Slug")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Domain.Models.Entities.Admission", b =>
                {
                    b.HasOne("Domain.Models.Entities.Student", "Student")
                        .WithMany("Admissions")
                        .HasForeignKey("StudentGuid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Models.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserGuid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Student");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Domain.Models.Entities.AdmissionChange", b =>
                {
                    b.HasOne("Domain.Models.Entities.Admission", "Admission")
                        .WithMany()
                        .HasForeignKey("AdmissionGuid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Admission");
                });

            modelBuilder.Entity("Domain.Models.Entities.AdmissionNote", b =>
                {
                    b.HasOne("Domain.Models.Entities.Admission", "Admission")
                        .WithMany("Notes")
                        .HasForeignKey("AdmissionGuid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Admission");
                });

            modelBuilder.Entity("Domain.Models.Entities.Admission", b =>
                {
                    b.Navigation("Notes");
                });

            modelBuilder.Entity("Domain.Models.Entities.Student", b =>
                {
                    b.Navigation("Admissions");
                });
#pragma warning restore 612, 618
        }
    }
}
