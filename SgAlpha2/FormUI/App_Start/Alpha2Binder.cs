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

            var dayName = name + "_day";
            var monthName = name + "_month";
            var yearName = name + "_year";

            if (!vp.ContainsPrefix(dayName) && !vp.ContainsPrefix(monthName) && !vp.ContainsPrefix(yearName))
                return null;

            var dayValue = vp.GetValue(dayName);
            var monthValue = vp.GetValue(monthName);
            var yearValue = vp.GetValue(yearName);

            var modelState = bindingContext.ModelState;
            modelState.Add(dayName, new ModelState() { Value = dayValue });
            modelState.Add(monthName, new ModelState() { Value = monthValue });
            modelState.Add(yearName, new ModelState() { Value = yearValue });

            int day;
            int month;
            int year;

            if (!int.TryParse(dayValue.AttemptedValue, out day) || !int.TryParse(monthValue.AttemptedValue, out month) || !int.TryParse(yearValue.AttemptedValue, out year))
                return null;

            return new DateTime(year, month, day);
        }
    }
}