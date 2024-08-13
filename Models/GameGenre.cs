namespace GameManagementMvc.Models
{
    public class GameGenre
    {
        // primary keys of many-to-many relationship
        public int GameId { get; set; }
        public int GenreId { get; set; }

        // navigation from join entity
        public Genre Genre { get; set; } = null!;
        public Game Game { get; set; } = null!;
    }
}
