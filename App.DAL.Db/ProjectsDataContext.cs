﻿using App.DAL.Db.Model;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.Db
{
    public class ProjectsDataContext : DbContext
    {
        public ProjectsDataContext(DbContextOptions<ProjectsDataContext> options)
            : base(options)
        {
        }

        public DbSet<Foo> Foos { get; set; }

        public DbSet<Bar> Bars { get; set; }

        public DbSet<Project> Projects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Foo>()
                .HasOne(e => e.Project)
                .WithMany(e => e.Foos)
                .HasForeignKey(e => e.ProjectId);

            modelBuilder.Entity<Bar>()
                .HasOne(e => e.Project)
                .WithMany(e => e.Bars)
                .HasForeignKey(e => e.ProjectId);

        }
    }
}
