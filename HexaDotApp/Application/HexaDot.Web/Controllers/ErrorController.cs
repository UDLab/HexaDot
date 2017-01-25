using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace HexaDot.Web.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Error404()
        {
            return View("404");
        }
    }
}
