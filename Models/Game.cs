using System.ComponentModel.DataAnnotations;

namespace GameManagementMvc.Models
{
    public class Game
    {
        // primary key
        public int Id { get; set; }

        // required foreign key property
        public int CompanyId { get; set; }

        // required, reference navigation to parent
        public Company Company { get; set; } = null!;

        // not required, populate to display in view
        public ICollection<Genre>? Genres { get; set; }

        [StringLength(64, MinimumLength = 1)]
        public required string Title { get; set; }

        [StringLength(2048, MinimumLength = 1)]
        public required string Body { get; set; }

        [StringLength(2048)]
        public string? Image { get; set; } // optional

        [Range(1, 5)]
        public required int Rating { get; set; }

        [DataType(DataType.Date)]
        public required DateTime ReleaseDate { get; set; }
    }
}
