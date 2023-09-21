using Microsoft.AspNetCore.Mvc;

namespace mvc.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
