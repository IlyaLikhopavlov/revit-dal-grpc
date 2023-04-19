﻿// <auto-generated />
using System;
using App.DAL.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace App.DAL.Db.Migrations
{
    [DbContext(typeof(ProjectsDataContext))]
    [Migration("20230221050714_MovedDescriptionToBaseEntity")]
    partial class MovedDescriptionToBaseEntity
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.2");

            modelBuilder.Entity("App.DAL.Db.Model.BaseEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Elevation")
                        .HasColumnType("TEXT");

                    b.Property<int>("EntityType")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("Guid")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<int?>("ProjectId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("BaseEntity", (string)null);

                    b.HasDiscriminator<int>("EntityType");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("App.DAL.Db.Model.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Elevation")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("App.DAL.Db.Model.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .HasColumnType("TEXT");

                    b.Property<string>("UniqueId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("App.DAL.Db.Model.Bar", b =>
                {
                    b.HasBaseType("App.DAL.Db.Model.BaseEntity");

                    b.Property<int>("CategoryId")
                        .HasColumnType("INTEGER");

                    b.HasIndex("CategoryId");

                    b.HasIndex("ProjectId");

                    b.ToTable("BaseEntity", (string)null);

                    b.HasDiscriminator().HasValue(2);
                });

            modelBuilder.Entity("App.DAL.Db.Model.Foo", b =>
                {
                    b.HasBaseType("App.DAL.Db.Model.BaseEntity");

                    b.HasIndex("ProjectId");

                    b.ToTable("BaseEntity", (string)null);

                    b.HasDiscriminator().HasValue(1);
                });

            modelBuilder.Entity("App.DAL.Db.Model.Bar", b =>
                {
                    b.HasOne("App.DAL.Db.Model.Category", "Category")
                        .WithMany("Bars")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("App.DAL.Db.Model.Project", "Project")
                        .WithMany("Bars")
                        .HasForeignKey("ProjectId");

                    b.Navigation("Category");

                    b.Navigation("Project");
                });

            modelBuilder.Entity("App.DAL.Db.Model.Foo", b =>
                {
                    b.HasOne("App.DAL.Db.Model.Project", "Project")
                        .WithMany("Foos")
                        .HasForeignKey("ProjectId");

                    b.Navigation("Project");
                });

            modelBuilder.Entity("App.DAL.Db.Model.Category", b =>
                {
                    b.Navigation("Bars");
                });

            modelBuilder.Entity("App.DAL.Db.Model.Project", b =>
                {
                    b.Navigation("Bars");

                    b.Navigation("Foos");
                });
#pragma warning restore 612, 618
        }
    }
}
