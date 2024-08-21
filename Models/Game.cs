using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GameManagementMvc.Models
{
    public class Game
    {
        // PROPERTIES
        public int Id { get; set; }

        public bool IsActive { get; set; } = true;

        [Required]
        [StringLength(64)]
        public required string Title { get; set; }

        [Required]
        [StringLength(2048)]
        public required string Body { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        [Display(Name = "Release Date")]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }

        // optional
        [Url]
        [StringLength(2048)]
        public string? Image { get; set; }

        // NAVIGATIONS PROPERTIES
        [JsonIgnore]
        [Display(Name = "Genres")]
        public ICollection<GameGenre> GameGenres { get; set; } = new List<GameGenre>();

        [JsonIgnore]
        [Display(Name = "Companies")]
        public ICollection<GameCompany> GameCompanies { get; set; } = new List<GameCompany>();
    }
}
