using Microsoft.AspNetCore.Mvc;

namespace GameManagementMvc.Controllers;

public class GameController : Controller
{
    public IActionResult Index()
    {
        ViewData["Title"] = "Game";

        return View();
    }
}
