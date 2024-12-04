using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RazorPagesMovie.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public UserRole Role { get; set; } = UserRole.Standard;
        
        [Timestamp]
        public byte[]? Timestamp { get; set; }
        
        public virtual ICollection<Movie> Movies { get; set; } = new List<Movie>();
    }
}
