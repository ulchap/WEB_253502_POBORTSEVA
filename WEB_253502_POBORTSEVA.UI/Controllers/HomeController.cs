using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WEB_253502_POBORTSEVA.UI.Models;

namespace WEB_253502_POBORTSEVA.UI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Header"] = "Лабораторная работа 2";

            var items = new List<ListDemo>
            {
                new ListDemo {Id = 1, Name = "Элемент 1"},
                new ListDemo {Id = 2, Name = "Элемент 2"},
                new ListDemo {Id = 3, Name = "Элемент 3"}
            };

            ViewBag.Items = new SelectList(items, "Id", "Name");

            return View();
        }
    }
}
