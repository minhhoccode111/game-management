using Microsoft.AspNetCore.Mvc;

namespace GameManagementMvc.Controllers;

public class PrivacyController : Controller
{
    public IActionResult Index()
    {
        ViewData["Title"] = "Privacy";

        return View();
    }
}
