namespace GameManagementMvc.Models
{
    public class GenreViewModel
    {
        public List<Genre>? Genres { get; set; }
        public List<Game>? Games { get; set; }
        public Genre? Genre { get; set; }
        public string? SearchTitle { get; set; }
        public string? SortBy { get; set; }
    }
}
