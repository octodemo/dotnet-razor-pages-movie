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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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
                entity.Property(m => m.Timestamp).IsConcurrencyToken();
                entity.Property(m => m.Timestamp).HasDefaultValue(new byte[8]); // Set default value for Timestamp
            });

            // Seed data with hashed passwords
            var hashedAdminPw = HashPassword("admin123");
            var hashedUserPw = HashPassword("user123");

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Username = "admin",
                    Password = "password",
                    Role = UserRole.Admin,
                    Timestamp = new byte[8] // Initialize with empty timestamp
                },
                new User
                {
                    Id = 2,
                    Username = "user",
                    Password = "password",
                    Role = UserRole.Standard,
                    Timestamp = new byte[8] // Initialize with empty timestamp
                }
            );
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