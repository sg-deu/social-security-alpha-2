using System;
using System.Web.Mvc;

namespace FormUI.Controllers.Helpers.Controls
{
    public abstract class InputControl : Control
    {
        protected ControlContext        ControlContext;
        protected ModelStateDictionary  ModelState;

        public InputControl(HtmlHelper helper, ControlContext controlContext) : base(helper)
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
            if (!ModelState.ContainsKey(name))
                return null;

            var value = ModelState[name];

            if (value.Value == null)
                return null;

            return value.Value.AttemptedValue;
        }

        protected string RenderValue()
        {
            return ModelStateValue(ControlContext.Name) ?? ModelValue();
        }
    }
}