using System;
using System.Web.Mvc;
using FormUI.Controllers.Helpers;
using FormUI.Domain.Util;

namespace FormUI.Controllers.Shared
{
    public class FormController : Controller
    {
        protected ActionResult Exec(Command cmd, Func<ActionResult> success, Func<ActionResult> failure)
        {
            return Exec<object>(ModelState, () => { PresentationRegistry.NewExecutor(ModelState.IsValid).Execute(cmd); return null; }, nullValue => success(), failure);
        }

        protected ActionResult Exec<T>(Command<T> cmd, Func<T, ActionResult> success, Func<ActionResult> failure)
        {
            return Exec(ModelState, () => PresentationRegistry.NewExecutor(ModelState.IsValid).Execute(cmd), success, failure);
        }

        protected ActionResult Exec<T>(Query<T> query, Func<T, ActionResult> success, Func<ActionResult> failure)
        {
            return Exec(ModelState, () => PresentationRegistry.NewExecutor(ModelState.IsValid).Execute(query), success, failure);
        }

        private static ActionResult Exec<TReturn>(ModelStateDictionary modelState, Func<TReturn> run, Func<TReturn, ActionResult> success, Func<ActionResult> failure)
        {
            TReturn response = default(TReturn);

            try
            {
                response = run();
            }
            catch (DomainException exception)
            {
                bool addedErrorReason = false;

                foreach (var nonPropertyError in exception.Messages)
                {
                    modelState.AddModelError("", nonPropertyError);
                    addedErrorReason = true;
                }

                foreach (var propertyError in exception.PropertyErrors)
                {
                    var key = propertyError.Key.GetExpressionText();
                    modelState.AddModelError(key, propertyError.Value);
                    addedErrorReason = true;
                }

                if (!addedErrorReason)
                    modelState.AddModelError("", "The application has encountered an unknown error.  Please help us by letting us know about the problem.");
            }

            return modelState.IsValid
                ? success(response)
                : failure();
        }
    }
}