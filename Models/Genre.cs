using System.ComponentModel.DataAnnotations;

namespace GameManagementMvc.Models
{
    public class Genre
    {
        public int Id { get; set; }

        [StringLength(32, MinimumLength = 2)]
        public string Title { get; set; } = null!;

        [StringLength(2048)]
        public string Body { get; set; } = null!;

        // not required, populate to display in view
        public ICollection<Game>? Games { get; set; }
    }
}
