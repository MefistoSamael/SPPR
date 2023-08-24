using Microsoft.AspNetCore.Mvc;
using WEB_153501_BYCHKO.ViewModels;

namespace WEB_153501_BYCHKO.Controllers
{
    public class Home : Controller
    {
        public IActionResult Index()
        {
            ViewData["text"] = "Лабораторная работа 2";
            var vm = new DemoViewModel();
            vm.SelectedId = 0;
            return View(vm);
        }
    }
}
