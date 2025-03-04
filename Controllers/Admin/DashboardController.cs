using Microsoft.AspNetCore.Mvc;

namespace TTDN.Controllers.Admin
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
