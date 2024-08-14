using System.ComponentModel.DataAnnotations;

namespace GameManagementMvc.Models
{
    public class Company
    {
        // PROPERTIES
        public int Id { get; set; }

        [StringLength(64)]
        public required string Title { get; set; }

        [StringLength(2048)]
        public required string Body { get; set; }

        [DataType(DataType.Date)]
        public DateTime FoundingDate { get; set; }

        // optional
        [StringLength(2048)]
        public string? Image { get; set; }

        // NAVIGATION PROPERTIES
        public ICollection<GameCompany> GameCompanies { get; set; } = new List<GameCompany>();
    }
}
