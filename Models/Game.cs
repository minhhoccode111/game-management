using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameManagementMvc.Models
{
    public class Game
    {
        public int Id { get; set; }

        [StringLength(64, MinimumLength = 2)]
        [Required]
        public string? Title { get; set; }

        [StringLength(2048)]
        [Required]
        public string? Body { get; set; }

        // this not required
        [StringLength(2048)]
        public string? Image { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        [DataType(DataType.Date)]
        [Required]
        public DateTime ReleaseDate { get; set; }

        [Required]
        public int CompanyId { get; set; }

        [NotMapped]
        public Company? Company { get; set; }

        [Required]
        public List<int>? GenreIds { get; set; } = new List<int>();

        // not mean we don't map the real Genre Model to this field reduce the
        // size but each time we retrieve Game model, we have to populate the
        // Genres with GenreIds
        [NotMapped]
        public List<Genre> Genres { get; set; } = new List<Genre>();
    }
}
