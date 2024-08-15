using Microsoft.AspNetCore.Mvc.Rendering;

namespace GameManagementMvc.Models
{
    public class GameFormViewModel
    {
        public MultiSelectList Genres { get; set; } = null!;
        public List<Company> Companies { get; set; } = null!;
        // TODO:
    }
}
