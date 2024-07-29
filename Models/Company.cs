using System.ComponentModel.DataAnnotations;

namespace GameManagementMvc.Models
{
    public class Company
    {
        public int Id { get; set; }

        [StringLength(64, MinimumLength = 2)]
        [Required]
        public string? Title { get; set; }

        [StringLength(2048)]
        [Required]
        public string? Body { get; set; }

        [StringLength(2048)]
        public string? Image { get; set; }

        // public ICollection<Game>? Games { get; set; }

        [DataType(DataType.Date)]
        public DateTime FoundingDate { get; set; }
    }
}