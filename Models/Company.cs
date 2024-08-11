using System.ComponentModel.DataAnnotations;

namespace GameManagementMvc.Models
{
    public class Company
    {
        public int Id { get; set; }

        [StringLength(64, MinimumLength = 2)]
        public string Title { get; set; } = null!;

        [StringLength(2048)]
        public string Body { get; set; } = null!;

        [StringLength(2048)]
        public string? Image { get; set; } // optional

        [DataType(DataType.Date)]
        public DateTime FoundingDate { get; set; }

        // collection navigation containing children
        public ICollection<Game> Games { get; set; } = null!;
    }
}
