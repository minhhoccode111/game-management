using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GameManagementMvc.Models
{
    public class GameCompany
    {
        // PROPERTIES
        public int Id { get; set; }

        public int GameId { get; set; }

        public int CompanyId { get; set; }

        [Required]
        [StringLength(64)]
        public required string Title { get; set; }

        [Required]
        [StringLength(2048)]
        public required string Body { get; set; }

        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        // optional
        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        // NAVIGATION PROPERTIES
        [JsonIgnore]
        public Game Game { get; set; } = null!;

        [JsonIgnore]
        public Company Company { get; set; } = null!;
    }
}
