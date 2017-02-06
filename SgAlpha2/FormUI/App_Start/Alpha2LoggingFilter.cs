using System.Diagnostics;
using System.Web.Mvc;

namespace FormUI.App_Start
{
    public class Alpha2LoggingFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            Trace.TraceInformation("Alpha2::Information OnActionExecuted {0}", filterContext.ActionDescriptor.ActionName);
            Trace.TraceWarning("Alpha2::Warning OnActionExecuted {0}", filterContext.ActionDescriptor.ActionName);
            Trace.TraceError("Alpha2::Error OnActionExecuted {0}", filterContext.ActionDescriptor.ActionName);
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
        }
    }
}