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

        // many-to-many with Game through GameGenre
        public List<Game> Games { get; set; } = null!;
    }
}
