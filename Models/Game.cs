using System.ComponentModel.DataAnnotations;

namespace GameManagementMvc.Models
{
    public class Game
    {
        // primary key
        public int Id { get; set; }

        // required, foreign key property
        public int CompanyId { get; set; }

        // navigation parent
        // not required, populate to display in view
        public Company? Company { get; set; }

        // many-to-many relationship with Genre through GameGenre class for join
        // entity
        public List<Genre> Genres { get; } = [];

        // navigation to join entity
        public List<GameGenre> GameGenres { get; } = [];

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
