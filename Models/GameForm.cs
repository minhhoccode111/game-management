namespace GameManagementMvc.Models
{
    public class GameForm : GameParent
    {
        public int CompanyId { get; set; }

        public List<int> GenreIds { get; set; } = null!;
    }
}
