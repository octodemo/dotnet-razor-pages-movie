using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RazorPagesMovie.Models
{
    public class Review
    {
        public int Id { get; set; }

        [Required]
        public int MovieId { get; set; }
        [ForeignKey("MovieId")] // Add ForeignKey attribute
        public Movie? Movie { get; set; } // Navigation property

        [Required]
        public int UserId { get; set; }
        [ForeignKey("UserId")] // Add ForeignKey attribute
        public User? User { get; set; } // Navigation property

        [Range(1, 5)]
        public int Rating { get; set; }

        public string Comment { get; set; } = string.Empty;

        [Timestamp]
        public byte[] Timestamp { get; set; } = new byte[8];
    }
}