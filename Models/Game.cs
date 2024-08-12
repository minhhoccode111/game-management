using System.ComponentModel.DataAnnotations;

namespace GameManagementMvc.Models
{
    public class Game
    {
        public int Id { get; set; }

        // one-to-many with Company
        public int CompanyId { get; set; }
        public Company Company { get; set; } = null!;

        // many-to-many with Genre through GameGenre
        public List<Genre> Genres { get; set; } = [];
        public List<GameGenre> GameGenres { get; } = [];

        // handle form submit GenreIds checkboxes
        public List<int>? GenreIds { get; set; }

        [Required]
        [StringLength(64)]
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
