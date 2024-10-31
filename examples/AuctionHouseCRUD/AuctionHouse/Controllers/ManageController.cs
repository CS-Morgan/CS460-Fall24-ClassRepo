using Microsoft.AspNetCore.Mvc;

namespace AuctionHouse.Controllers
{
    public class ManageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
