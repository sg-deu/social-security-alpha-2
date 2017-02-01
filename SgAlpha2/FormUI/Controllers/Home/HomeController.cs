using System.Web.Mvc;
using FormUI.App_Start;

namespace FormUI.Controllers.Home
{
    public static class HomeActions
    {
        public static string Index()    { return "~/"; }
        public static string Password() { return "~/home/password/"; }
    }

    public class HomeController : Controller
    {
        public const string PasswordReturnUrlName   = "returnUrl";
        public const string PasswordValue           = "PinkElephant1";

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

        [HttpPost]
        public ActionResult Password(string password)
        {
            if (password != PasswordValue)
                return Redirect(HttpContext.Request.Url.OriginalString);

            Alpha2EntryFilter.Authenticate(Response);

            var returnUrl = Request.QueryString[PasswordReturnUrlName] ?? HomeActions.Index();
            return Redirect(returnUrl);
        }
    }
}