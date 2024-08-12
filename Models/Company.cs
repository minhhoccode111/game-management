using System.ComponentModel.DataAnnotations;

namespace GameManagementMvc.Models
{
    public class Company
    {
        public int Id { get; set; }

        [Required]
        [StringLength(64)]
        public required string Title { get; set; }

        [Required]
        [StringLength(2048)]
        public required string Body { get; set; }

        // optional
        [StringLength(2048)]
        public string? Image { get; set; }

        [DataType(DataType.Date)]
        public required DateTime FoundingDate { get; set; }

        // not required, one-to-many relationship with Game
        public List<Game> Games { get; set; } = null!;
    }
}
