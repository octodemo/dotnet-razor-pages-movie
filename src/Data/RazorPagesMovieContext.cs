using Microsoft.EntityFrameworkCore;
using RazorPagesMovie.Models;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace RazorPagesMovie.Data
{
    public class RazorPagesMovieContext : DbContext
    {
        public RazorPagesMovieContext(DbContextOptions<RazorPagesMovieContext> options)
            : base(options)
        {
        }

        public DbSet<Movie> Movie { get; set; } = default!;
        public DbSet<User> Users { get; set; } = default!;
        public DbSet<Director> Directors { get; set; } = default!;
        public DbSet<Review> Reviews { get; set; } = default!;
        public DbSet<Artist> Artists { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Director configuration
            modelBuilder.Entity<Director>(entity =>
            {
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.BirthDate).IsRequired();
            });

            // Artist configuration
            modelBuilder.Entity<Artist>(entity =>
            {
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.BirthDate).IsRequired();
                entity.Property(e => e.Nationality).HasMaxLength(50);
            });
            
            // User configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Username).IsUnique();
                entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Password).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Role).IsRequired();
                entity.Property(e => e.Timestamp).IsConcurrencyToken();
            });

            // Movie configuration
            modelBuilder.Entity<Movie>(entity =>
            {
                entity.HasOne(m => m.User)
                    .WithMany(u => u.Movies)
                    .HasForeignKey(m => m.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(m => m.UserId);
                entity.Property(m => m.Timestamp).IsRowVersion(); // Ensure Timestamp is configured as rowversion
            });

            // Seed data with hashed passwords
            var hashedAdminPw = HashPassword("password");
            var hashedUserPw = HashPassword("password");

            // Seed admin users
            modelBuilder.Entity<User>().HasData(new User
            {
                Id = 1,
                Username = "admin",
                Password = "password",
                Role = UserRole.Admin,
                Timestamp = new byte[8]
            });

            for (int i = 2; i <= 10; i++)
            {
                modelBuilder.Entity<User>().HasData(new User
                {
                    Id = i,
                    Username = $"admin{i - 1}",
                    Password = "password",
                    Role = UserRole.Admin,
                    Timestamp = new byte[8]
                });
            }

            // Seed standard users
            modelBuilder.Entity<User>().HasData(new User
            {
                Id = 11,
                Username = "user",
                Password = "password",
                Role = UserRole.Standard,
                Timestamp = new byte[8]
            });

            for (int i = 12; i <= 20; i++)
            {
                modelBuilder.Entity<User>().HasData(new User
                {
                    Id = i,
                    Username = $"user{i - 11}",
                    Password = "password",
                    Role = UserRole.Standard,
                    Timestamp = new byte[8]
                });
            }

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.NoAction; // Or DeleteBehavior.Restrict
            }
        }

        private static string HashPassword(string password)
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            return $"{Convert.ToBase64String(salt)}:{hashed}";
        }
    }
}