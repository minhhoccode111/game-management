using System.ComponentModel.DataAnnotations;

namespace GameManagementMvc.Models;

public class Company
{
    public int Id { get; set; }

    [StringLength(64, MinimumLength = 2)]
    [Required]
    public string? Name { get; set; }

    [StringLength(2048)]
    [Required]
    public string? Info { get; set; }

    [StringLength(2048)]
    public string? Image { get; set; }

    [DataType(DataType.Date)]
    public DateTime CreateDate { get; set; }
}
