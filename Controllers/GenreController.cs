using Microsoft.AspNetCore.Mvc;

namespace GameManagementMvc.Controllers;

public class GenreController : Controller
{
    public IActionResult Index()
    {
        ViewData["Title"] = "Genre";

        return View();
    }
}
