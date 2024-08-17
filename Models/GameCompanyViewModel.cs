using System.ComponentModel.DataAnnotations;

namespace GameManagementMvc.Models
{
    // TODO: this is duplicated w/ GameCompany model, fix this
    public class GameCompanyViewModel
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
    }
}
