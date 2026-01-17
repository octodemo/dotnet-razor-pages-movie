using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RazorPagesMovie.Models
{
    public class Artist
    {
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }
        
        public string? Nationality { get; set; }
        
        [Timestamp]
        public byte[]? Timestamp { get; set; }
    }
}