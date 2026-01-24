using be_lspmpi.Models;
using be_lspmpi.Repositories;
using Microsoft.EntityFrameworkCore;

namespace be_lspmpi.Data
{
    public class AppDbContext : DbContext, IDBContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<ArticleCategory> ArticleCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Role)
                      .WithMany()
                      .HasForeignKey(e => e.RoleId);
                entity.HasOne(e => e.UserProfile)
                      .WithMany()
                      .HasForeignKey(e => e.UserProfileId);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasData(
                    new Role { Id = 1, Name = "SuperAdmin", Level = 1 },
                    new Role { Id = 2, Name = "Admin", Level = 2 },
                    new Role { Id = 3, Name = "User", Level = 3 }
                );
            });

            modelBuilder.Entity<UserProfile>(entity =>
            {
                entity.HasKey(e => e.Id);
            });

            modelBuilder.Entity<Article>(entity =>
            {
                entity.HasKey(e => e.Id);
            });
        }
    }
}