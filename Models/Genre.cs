using System.ComponentModel.DataAnnotations;

namespace GameManagementMvc.Models
{
    public class Genre
    {
        public int Id { get; set; }

        [StringLength(32, MinimumLength = 2)]
        [Required]
        public string? Title { get; set; }

        [StringLength(2048)]
        [Required]
        public string? Body { get; set; }

        // public ICollection<Game>? Games { get; set; }
    }
}
