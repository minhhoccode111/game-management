using System.Text.Json.Serialization;

namespace GameManagementMvc.Models
{
    public class GameGenre
    {
        // PROPERTIES
        public int GameId { get; set; }

        public int GenreId { get; set; }

        // NAVIGATIONS
        [JsonIgnore]
        public Genre Genre { get; set; } = null!;

        [JsonIgnore]
        public Game Game { get; set; } = null!;
    }
}
