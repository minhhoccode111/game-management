using System.ComponentModel.DataAnnotations;

namespace GameManagementMvc.Models
{
    public class Game
    {
        // primary key
        public int Id { get; set; }

        // required, foreign key
        public int CompanyId { get; set; }

        // reference one-to-many relationship with Company
        public Company Company { get; set; } = null!;

        // many-to-many relationship with Genre
        // collection navigation to Genre
        public List<Genre> Genres { get; set; } = new();

        // not required, handle checkboxes of genre ids form submit
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
