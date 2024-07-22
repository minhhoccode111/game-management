using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc;

namespace MvcMovie.Controllers;

public class HelloWorldController : Controller
{
    //
    // GET: /HelloWorld/
    public IActionResult Index()
    {
        return View();
    }

    //
    // GET: /HelloWorld/Welcome/
    public IActionResult Welcome(string name, int numTimes = 1)
    {
        ViewData["Message"] = $"Hello {name}";
        ViewData["NumTimes"] = numTimes;
        return View();
    }
}
/*

The third URL segment matched the route parameter id.
The Welcome method contains a parameter id that matched the URL template in the MapControllerRoute method.
The trailing ? (in id?) indicates the id parameter is optional.

*/
