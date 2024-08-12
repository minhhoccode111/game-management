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

        // not required, many-to-many relationship with Game
        public List<Game> Games { get; set; } = null!;
    }
}
