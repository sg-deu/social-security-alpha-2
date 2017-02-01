using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using FormUI.Controllers.Home;

namespace FormUI.App_Start
{
    /// <summary>
    /// This filter ensures a single password is entered before allowing entry to the Alpha site.  This
    /// is simply to prevent random access to a public site from the public, while still allowing the
    /// Alpha2 team to review the screens.
    /// </summary>
    public class Alpha2EntryFilter : IAuthenticationFilter
    {
        private const string _cookieName = "Alpha2Entry";
        private const string _cookieValue = "allow";

        private static string _passwordAction = HomeActions.Password();

        public void OnAuthentication(AuthenticationContext filterContext)
        {
            if (SkipAuthentication(filterContext))
                return;

            if (!AuthenticateSuccess(filterContext))
                HandleUnauthenticated(filterContext);
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            // OnAuthentication already called by this point, so do nothing here
        }

        public static Action<HttpResponseBase> Authenticate = (HttpResponseBase response) =>
        {
            var cookie = new HttpCookie(_cookieName, _cookieValue);
            cookie.Expires = DateTime.UtcNow + TimeSpan.FromHours(24);
            response.Cookies.Add(cookie);
        };

        private bool SkipAuthentication(AuthenticationContext context)
        {
            var request = context.HttpContext.Request;

            if (request.AppRelativeCurrentExecutionFilePath.ToLower() == _passwordAction || request.CurrentExecutionFilePath.ToLower() == _passwordAction)
                return true;

            return false;
        }

        private bool AuthenticateSuccess(AuthenticationContext context)
        {
            var request = context.HttpContext.Request;
            var entryCookie = request.Cookies[_cookieName];

            return entryCookie != null;
        }

        private void HandleUnauthenticated(AuthenticationContext context)
        {
            var url = string.Format("{0}?{1}={2}", _passwordAction, HomeController.PasswordReturnUrlName, HttpUtility.UrlEncode(context.HttpContext.Request.Url.OriginalString));
            var redirect = new RedirectResult(url);
            context.Result = redirect;
        }
    }
}