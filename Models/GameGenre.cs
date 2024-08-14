namespace GameManagementMvc.Models
{
    public class GameGenre
    {
        // PROPERTIES
        public int GameId { get; set; }

        public int GenreId { get; set; }

        // NAVIGATIONS
        public Genre Genre { get; set; } = null!;

        public Game Game { get; set; } = null!;
    }
}
