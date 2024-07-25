using System.ComponentModel.DataAnnotations;

namespace GameManagementMvc.Models;

public class Game
{
    public int Id { get; set; }

    [Range(1, 5)]
    public int Rating { get; set; }

    [StringLength(64, MinimumLength = 2)]
    [Required]
    public string? Name { get; set; }

    [StringLength(2048)]
    [Required]
    public string? Info { get; set; }

    // this not required
    [StringLength(2048)]
    public string? Image { get; set; }

    [DataType(DataType.Date)]
    [Required]
    public DateTime ReleaseDate { get; set; }

    public int CompanyId { get; set; }

    public List<int>? GenreIds { get; set; }
}
