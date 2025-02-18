using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RazorPagesMovie.Models
{
    public class Artist
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000)]
        public string Bio { get; set; } = string.Empty;

        [Timestamp]
        public byte[] Timestamp { get; set; } = new byte[8];

        public virtual ICollection<Movie> Movies { get; set; } = new List<Movie>();
    }
}
