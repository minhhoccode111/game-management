using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GameManagementMvc.Models
{
    public class Company
    {
        // PROPERTIES
        public int Id { get; set; }

        [Required]
        [StringLength(64)]
        public required string Title { get; set; }

        [Required]
        [StringLength(2048)]
        public required string Body { get; set; }

        [Display(Name = "Founding Date")]
        [DataType(DataType.Date)]
        public DateTime FoundingDate { get; set; }

        // optional
        [Url]
        [StringLength(2048)]
        public string? Image { get; set; }

        // NAVIGATION PROPERTIES
        [JsonIgnore]
        public ICollection<GameCompany> GameCompanies { get; set; } = new List<GameCompany>();
    }
}
