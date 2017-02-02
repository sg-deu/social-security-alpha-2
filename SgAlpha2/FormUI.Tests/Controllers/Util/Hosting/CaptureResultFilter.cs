using System.Web.Mvc;

namespace FormUI.Tests.Controllers.Util.Hosting
{
    public class CaptureResultFilter : ActionFilterAttribute
    {
        public static ResultExecutedContext LastResult;

        public static void Register()
        {
            GlobalFilters.Filters.Add(new CaptureResultFilter());
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            base.OnResultExecuted(filterContext);

            if (filterContext != null)
                LastResult = new ResultExecutedContext()
                {
                    Canceled            = filterContext.Canceled,
                    Controller          = filterContext.Controller,
                    Exception           = filterContext.Exception,
                    ExceptionHandled    = filterContext.ExceptionHandled,
                    HttpContext         = filterContext.HttpContext,
                    RequestContext      = filterContext.RequestContext,
                    Result              = filterContext.Result,
                    RouteData           = filterContext.RouteData,
                };
        }
    }
}