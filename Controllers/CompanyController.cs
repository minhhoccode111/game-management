using Microsoft.AspNetCore.Mvc;

namespace GameManagementMvc.Controllers;

public class CompanyController : Controller
{
    public IActionResult Index()
    {
        ViewData["Title"] = "Company";

        return View();
    }
}
