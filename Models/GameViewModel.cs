using Microsoft.AspNetCore.Mvc.Rendering;

namespace GameManagementMvc.Models
{
    public class GameViewModel
    {
        public List<Game>? Games { get; set; }
        public SelectList? Genres { get; set; }
        public SelectList? Companies { get; set; }
        public string? Title { get; set; }
        public string? Sort { get; set; }
        public int? Rating { get; set; }
        public int? GenreId { get; set; }
        public int? CompanyId { get; set; }
    }
}
