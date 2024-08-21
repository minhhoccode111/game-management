using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GameManagementMvc.Models
{
    public class Genre
    {
        // PROPERTIES
        public int Id { get; set; }

        public bool IsActive { get; set; } = true;

        [Required]
        [StringLength(32)]
        public required string Title { get; set; }

        [Required]
        [StringLength(2048)]
        public required string Body { get; set; }

        // NAVIGATION PROPERTIES
        [JsonIgnore]
        [Display(Name = "Games")]
        public ICollection<GameGenre> GameGenres { get; set; } = new List<GameGenre>();
    }
}
