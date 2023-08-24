using Microsoft.AspNetCore.Mvc;
using WEB_153501_BYCHKO.ViewModels;

namespace WEB_153501_BYCHKO.Controllers
{
    public class Home : Controller
    {
        DemoViewModel demoViewModel ;
        public IActionResult Index()
        {
            ViewData["text"] = "Лабораторная работа 2";
            demoViewModel = new DemoViewModel();
            return View(demoViewModel);
        }
    }
}
