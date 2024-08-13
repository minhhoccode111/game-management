using System.ComponentModel.DataAnnotations;

namespace GameManagementMvc.Models
{
    public class Game
    {
        public int Id { get; set; }

        // one-to-many with Company
        public Company Company { get; set; } = null!;

        // navigation to principal in one-to-many relationship
        public int CompanyId { get; set; }

        // many-to-many
        public List<Genre> Genres { get; } = [];

        // navigation to join entity
        public List<GameGenre> GameGenres { get; } = [];

        // handle checkboxes form submit
        public List<int>? GenreIds { get; set; }

        [Required]
        [StringLength(64, MinimumLength = 2)]
        public required string Title { get; set; }

        [Required]
        [StringLength(2048)]
        public required string Body { get; set; }

        // optional
        [StringLength(2048)]
        public string? Image { get; set; }

        [Range(1, 5)]
        public required int Rating { get; set; }

        [DataType(DataType.Date)]
        public required DateTime ReleaseDate { get; set; }
    }
}
