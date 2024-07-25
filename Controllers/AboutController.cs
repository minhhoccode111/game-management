using Microsoft.AspNetCore.Mvc;

namespace GameManagementMvc.Controllers;

public class AboutController : Controller
{
    public IActionResult Index()
    {
        ViewData["Title"] = "About";

        return View();
    }
}
