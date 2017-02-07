using System;
using System.Web.Mvc;

namespace FormUI.App_Start
{
    public class Alpha2Binder : IModelBinder
    {
        public static void Register()
        {
            ModelBinders.Binders[typeof(DateTime)] = new Alpha2Binder();
        }

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var value = BindDateTime(controllerContext, bindingContext);
            return value ?? ModelBinders.Binders.DefaultBinder.BindModel(controllerContext, bindingContext);
        }

        private DateTime? BindDateTime(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var name = bindingContext.ModelName;
            var vp = bindingContext.ValueProvider;

            if (!vp.ContainsPrefix(name + "_day") && !vp.ContainsPrefix(name + "_month") && !vp.ContainsPrefix(name + "_year"))
                return null;

            var dayText = vp.GetValue(name + "_day");
            var monthText = vp.GetValue(name + "_month");
            var yearText = vp.GetValue(name + "_year");

            int day;
            int month;
            int year;

            if (!int.TryParse(dayText.AttemptedValue, out day) || !int.TryParse(monthText.AttemptedValue, out month) || !int.TryParse(yearText.AttemptedValue, out year))
                return null;

            return new DateTime(year, month, day);
        }
    }
}