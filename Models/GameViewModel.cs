using Microsoft.AspNetCore.Mvc.Rendering;

namespace GameManagementMvc.Models
{
    public class GameViewModel
    {
        public List<Game>? Games { get; set; }
        public SelectList? Genres { get; set; }
        public SelectList? Companies { get; set; }
        public string? SearchTitle { get; set; }
        public string? SearchGenre { get; set; }
        public int? SearchRating { get; set; }
        public string? SearchCompany { get; set; }
    }
}
