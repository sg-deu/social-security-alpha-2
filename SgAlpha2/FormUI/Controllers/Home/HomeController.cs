using System.Web.Mvc;

namespace FormUI.Controllers.Home
{
    public static class HomeActions
    {
        public static string Index()    { return "~/"; }
        public static string Password() { return "~/home/password/"; }
    }

    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Password()
        {
            return View();
        }
    }
}