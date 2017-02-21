using System.Diagnostics;
using System.Text;
using System.Web.Mvc;

namespace FormUI.App_Start
{
    public class LoggingFilter : IActionFilter, IExceptionFilter, IResultFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var sb = new StringBuilder();
            var request = filterContext.HttpContext.Request;
            var method = request.HttpMethod;
            var url = request.Url.OriginalString;
            sb.AppendFormat("{0} {1}", method, url);
            Info(sb.ToString());
        }

        public void OnException(ExceptionContext filterContext)
        {
            var e = filterContext.Exception;

            if (e == null)
            {
                Error("OnException called, with no exception");
                return;
            }

            Error(e.ToString());
        }

        public void OnResultExecuting(ResultExecutingContext filterContext)
        {
        }

        public void OnResultExecuted(ResultExecutedContext filterContext)
        {
            var result = filterContext.Result;

            if (result == null)
            {
                Info("No result");
                return;
            }

            var sb = new StringBuilder();

            sb.AppendFormat("{0}", result.GetType().Name);

            var viewResult = result as ViewResult;

            if (viewResult != null)
            {
                var view = viewResult.View as RazorView;

                if (view != null)
                    sb.AppendFormat(" {0}", view.ViewPath);
            }

            Info(sb.ToString());
        }

        public static void InfoFormat(string format, params object[] args)
        {
            Info(string.Format(format, args));
        }

        public static void WarnFormat(string format, params object[] args)
        {
            Warn(string.Format(format, args));
        }

        public static void ErrorFormat(string format, params object[] args)
        {
            Error(string.Format(format, args));
        }

        public static void Info(string message)
        {
            Trace.TraceInformation($"Form_ui::INFO  {message}");
        }

        public static void Warn(string message)
        {
            Trace.TraceWarning($"Form_ui::WARN  {message}");
        }

        public static void Error(string message)
        {
            Trace.TraceError($"Form_ui::ERROR {message}");
        }
    }
}