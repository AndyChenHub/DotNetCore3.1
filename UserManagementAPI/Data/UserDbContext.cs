using System;
using Microsoft.EntityFrameworkCore;
using UserManagementAPI.Models;

namespace UserManagementAPI.Data
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Client> Clients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure User entity
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users", "Domain");

                entity.HasKey(e => e.UserId)
                    .HasName("PK_Domain.Users");

                entity.Property(e => e.UserId)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.UserName)
                    .HasMaxLength(200)
                    .IsRequired();

                entity.Property(e => e.Email)
                    .HasMaxLength(1000)
                    .IsRequired();

                entity.Property(e => e.Alias)
                    .HasMaxLength(1000);

                entity.Property(e => e.FirstName)
                    .HasMaxLength(1000);

                entity.Property(e => e.LastName)
                    .HasMaxLength(1000);

                entity.Property(e => e.UserType)
                    .IsRequired()
                    .HasConversion(
                        v => v.ToString(),   // Convert enum to string for database storage
                        v => (UserType)Enum.Parse(typeof(UserType), v));
            });

            // Configure Manager entity
            modelBuilder.Entity<Manager>(entity =>
            {
                entity.ToTable("Managers", "Domain");

                entity.HasKey(e => e.ManagerId)
                    .HasName("PK_Domain.Managers");

                entity.Property(e => e.ManagerId)
                    .ValueGeneratedNever();

                entity.Property(e => e.Position)
                    .HasMaxLength(50)
                    .IsRequired();

                entity.HasOne(e => e.User)
                    .WithOne(u => u.Manager)
                    .HasForeignKey<Manager>(e => e.ManagerId)
                    .HasConstraintName("FK_Domain.Managers_Users");
            });

            // Configure Client entity
            modelBuilder.Entity<Client>(entity =>
            {
                entity.ToTable("Clients", "Domain");

                entity.HasKey(e => e.ClientId)
                    .HasName("PK_Domain.Clients");

                entity.Property(e => e.ClientId)
                    .ValueGeneratedNever();

                entity.Property(e => e.Level)
                    .IsRequired();

                entity.HasOne(e => e.User)
                    .WithOne(u => u.Client)
                    .HasForeignKey<Client>(e => e.ClientId)
                    .HasConstraintName("FK_Domain.Clients_Users");

                entity.HasOne(e => e.Manager)
                    .WithMany(m => m.Clients)
                    .HasForeignKey(e => e.ManagerId)
                    .HasConstraintName("FK_Domain.Clients_Managers")
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

    }
}
