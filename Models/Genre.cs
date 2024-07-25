using System.ComponentModel.DataAnnotations;

namespace GameManagementMvc.Models;

public class Genre
{
    public int Id { get; set; }

    [StringLength(32, MinimumLength = 2)]
    [Required]
    public string? Name { get; set; }
}
