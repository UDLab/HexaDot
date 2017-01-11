namespace HexaDot.Features.Home
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : Controller
    {
      

        public HomeController()
        {
            
        }
        public IActionResult Index()
        {
            ViewData["Title"] = "Start Page";
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
