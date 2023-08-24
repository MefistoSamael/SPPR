using Microsoft.AspNetCore.Mvc;

namespace WEB_153501_BYCHKO.Controllers
{
    public class Home : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
