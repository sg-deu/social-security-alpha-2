using System;
using System.Web.Mvc;

namespace FormUI.Controllers.Helpers.Controls
{
    public abstract class InputControl : Control
    {
        protected ControlContext        ControlContext;
        protected ModelStateDictionary  ModelState;

        public InputControl(ControlContext controlContext)
        {
            ControlContext = controlContext;
            ModelState = controlContext.Helper.ViewData.ModelState;
        }

        protected virtual string ModelValue()
        {
            return Convert.ToString(ControlContext.Metadata.Model);
        }

        protected string ModelStateValue(string name)
        {
            return ModelState.ContainsKey(name)
                ? ModelState[name].Value.AttemptedValue
                : null;
        }

        protected string RenderValue()
        {
            return ModelStateValue(ControlContext.Name) ?? ModelValue();
        }
    }
}