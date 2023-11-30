using Microsoft.AspNetCore.Mvc;

namespace BethanysPieShopAdmin.Data.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
