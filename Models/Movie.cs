using System.ComponentModel.DataAnnotations;

namespace MvcMovie.Models;

public class Movie
{
    public int Id { get; set; }
    public string? Title { get; set; }

    /*
    DataType attribute on ReleaseDate specifies the type of the data (`Date`) with this attribute
        the user isn't required to enter time information in the date field
        only the date is displayed, not time information
    */

    [DataType(DataType.Date)]
    public DateTime ReleaseDate { get; set; }

    // string can be nullable
    public string? Genre { get; set; }
    public decimal Price { get; set; }
}
