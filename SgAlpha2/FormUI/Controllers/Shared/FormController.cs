﻿using System;
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
                foreach (var nonPropertyError in exception.Messages)
                    modelState.AddModelError("", nonPropertyError);

                foreach (var propertyError in exception.PropertyErrors)
                {
                    var key = propertyError.Key.GetExpressionText();
                    modelState.AddModelError(key, propertyError.Value);
                }
            }

            return modelState.IsValid
                ? success(response)
                : failure();
        }
    }
}