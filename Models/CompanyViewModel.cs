namespace GameManagementMvc.Models
{
    public class CompanyViewModel
    {
        public List<Company>? Companies { get; set; }
        public List<Game>? Games { get; set; }
        public Company? Company { get; set; }
        public string? SearchTitle { get; set; }
        public string? SortBy { get; set; }
    }
}
