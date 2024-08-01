using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        // use to populate when need to display game belong to this
        [NotMapped]
        public List<Game>? Games { get; set; }
    }
}
