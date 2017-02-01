using System.Web.Mvc;

namespace FormUI.Controllers
{
    public static class HomeActions
    {
        public static string Index() { return "~/"; }
        public static string Password() { return "~/home/password/"; }
    }

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Password()
        {
            return Content("this resource is password protected");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}