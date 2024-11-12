using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RazorPagesMovie.Models;

namespace RazorPagesMovie.Models
{
    public class Movie
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }

        [Required]
        public string Genre { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }

        [Required]
        public string Rating { get; set; }

        public int? UserId { get; set; }
        public User? User { get; set; }

        [Timestamp]
        public byte[] Timestamp { get; set; } = new byte[8]; // Initialize with a default value
    }
}