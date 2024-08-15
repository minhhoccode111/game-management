using Microsoft.AspNetCore.Mvc.Rendering;

namespace GameManagementMvc.Models
{
    public class GameViewModel
    {
        // data for table
        public List<Game> Games { get; set; } = null!;

        // select list dynamic data for filter
        public SelectList Genres { get; set; } = null!;
        public SelectList Companies { get; set; } = null!;

        // search queries user input
        public string? Title { get; set; }
        public string? Sort { get; set; }
        public int? Rating { get; set; }

        // search queries select list
        public int? GenreId { get; set; }
        public int? CompanyId { get; set; }
    }
}
