using System.ComponentModel.DataAnnotations;

namespace GameManagementMvc.Models
{
    public class Genre
    {
        public int Id { get; set; }

        // many-to-many
        public List<Game> Games { get; } = [];

        // navigation to join entity
        public List<GameGenre> GameGenres { get; } = [];

        [Required]
        [StringLength(32, MinimumLength = 2)]
        public required string Title { get; set; }

        [Required]
        [StringLength(2048)]
        public required string Body { get; set; }
    }
}
