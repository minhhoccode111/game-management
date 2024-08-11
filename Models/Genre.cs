using System.ComponentModel.DataAnnotations;

namespace GameManagementMvc.Models
{
    public class Genre
    {
        public int Id { get; set; }

        [Required]
        [StringLength(32)]
        public required string Title { get; set; }

        [Required]
        [StringLength(2048)]
        public required string Body { get; set; }

        // many-to-many relationship with Game through GameGenre class for join
        // entity
        public List<Game> Games { get; } = [];

        // navigation to join entity
        public List<GameGenre> GameGenres { get; } = [];
    }
}
