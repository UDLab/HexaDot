using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MediatR;


namespace HexaDot.Web.Controllers
{
   
    public class HomeController : Controller
    {
        
    
        public HomeController()
        {

        }
        public IActionResult Index()
        {
            ViewData["Title"] = "Home";
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "HexaDot";

            return View();
        }
           
        public IActionResult Contact()
        {
            ViewData["Message"] = "Contact";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
