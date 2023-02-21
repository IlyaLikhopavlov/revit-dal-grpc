using App.DAL.Db.Model;
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

        public DbSet<Category> Categories { get; set; }

        public DbSet<Project> Projects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BaseEntity>()
                .ToTable("BaseEntity")
                .HasDiscriminator<int>("EntityType")
                .HasValue<Foo>(1)
                .HasValue<Bar>(2);

            modelBuilder.Entity<Foo>()
                .ToTable("BaseEntity")
                .HasOne(e => e.Project)
                .WithMany(e => e.Foos)
                .HasForeignKey(e => e.ProjectId);

            modelBuilder.Entity<Bar>()
                .ToTable("BaseEntity")
                .HasOne(e => e.Project)
                .WithMany(e => e.Bars)
                .HasForeignKey(e => e.ProjectId);

            modelBuilder.Entity<Category>()
                .HasMany(c => c.Bars)
                .WithOne(b => b.Category)
                .HasForeignKey(b => b.CategoryId);

        }
    }
}
