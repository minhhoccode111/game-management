namespace GameManagementMvc.Models
{
    public class Game : GameParent
    {
        // one-to-many with Company
        public Company Company { get; set; } = null!;

        // many-to-many with Genre through GameGenre
        public List<Genre> Genres { get; set; } = null!;
    }
}
