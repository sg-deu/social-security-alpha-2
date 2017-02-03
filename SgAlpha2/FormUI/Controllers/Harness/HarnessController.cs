using System.Web.Mvc;

namespace FormUI.Controllers.Harness
{
    public class HarnessController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
    }
}