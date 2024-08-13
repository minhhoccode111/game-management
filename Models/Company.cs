using System.ComponentModel.DataAnnotations;

namespace GameManagementMvc.Models
{
    public class Company
    {
        public int Id { get; set; }

        // collection navigation containing dependents
        public ICollection<Game> Games { get; } = new List<Game>();

        [Required]
        [StringLength(64, MinimumLength = 2)]
        public required string Title { get; set; }

        [Required]
        [StringLength(2048)]
        public required string Body { get; set; }

        // optional
        [StringLength(2048)]
        public string? Image { get; set; }

        [DataType(DataType.Date)]
        public required DateTime FoundingDate { get; set; }
    }
}
