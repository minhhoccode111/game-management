using System.ComponentModel.DataAnnotations;

namespace GameManagementMvc.Models
{
    public class GameCompany
    {
        // PROPERTIES
        public int Id { get; set; }

        public int GameId { get; set; }

        public int CompanyId { get; set; }

        [StringLength(64)]
        public required string Title { get; set; }

        [StringLength(2048)]
        public required string Body { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        // optional
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        // NAVIGATION PROPERTIES

        public Game Game { get; set; } = null!;

        public Company Company { get; set; } = null!;
    }
}
