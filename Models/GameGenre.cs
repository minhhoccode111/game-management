namespace GameManagementMvc.Models
{
    public class GameGenre
    {
        public int GameId { get; set; }
        public int GenreId { get; set; }

        // navigation from join entity
        public Game Game { get; set; } = null!;
        public Genre Genre { get; set; } = null!;
    }
}
