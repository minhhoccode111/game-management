using System.ComponentModel.DataAnnotations;

namespace GameManagementMvc.Models
{
    public class Genre
    {
        // PROPERTIES
        public int Id { get; set; }

        [Required]
        [StringLength(32)]
        public required string Title { get; set; }

        [Required]
        [StringLength(2048)]
        public required string Body { get; set; }

        // NAVIGATION PROPERTIES
        public ICollection<GameGenre> GameGenres { get; set; } = new List<GameGenre>();
    }
}
