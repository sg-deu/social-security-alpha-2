﻿using System;
using System.Globalization;
using System.Web.Mvc;

namespace FormUI.App_Start
{
    public class Binder
    {
        public static void Register()
        {
            ModelBinders.Binders[typeof(DateTime)] = new DateTimeBinder();
            ModelBinders.Binders[typeof(DateTime?)] = new DateTimeBinder();
            ModelBinders.Binders[typeof(int)] = new IntBinder();
            ModelBinders.Binders[typeof(int?)] = new IntBinder();
        }

        public class IntBinder : IModelBinder
        {
            public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
            {
                var value = BindInt(controllerContext, bindingContext);
                return value;
            }

            private int? BindInt(ControllerContext controllerContext, ModelBindingContext bindingContext)
            {
                var name = bindingContext.ModelName;
                var vp = bindingContext.ValueProvider;

                if (!vp.ContainsPrefix(name))
                    return null;

                var vpr = vp.GetValue(name);
                bindingContext.ModelState.Add(name, new ModelState { Value = vpr });

                if (string.IsNullOrWhiteSpace(vpr.AttemptedValue))
                    return null;

                int value;

                if (!int.TryParse(vpr.AttemptedValue, out value))
                {
                    bindingContext.ModelState.AddModelError(name, $"{bindingContext.ModelMetadata.GetDisplayName()}: please supply a valid number");
                    return null;
                }

                return value;
            }
        }

        public class DateTimeBinder : IModelBinder
        {
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

                if (!vp.ContainsPrefix(dayName) || !vp.ContainsPrefix(monthName) || !vp.ContainsPrefix(yearName))
                    return null;

                var dayValue = vp.GetValue(dayName);
                var monthValue = vp.GetValue(monthName);
                var yearValue = vp.GetValue(yearName);

                var modelState = bindingContext.ModelState;
                modelState.Add(dayName, new ModelState { Value = dayValue });
                modelState.Add(monthName, new ModelState { Value = monthValue });
                modelState.Add(yearName, new ModelState { Value = yearValue });

                if (string.IsNullOrWhiteSpace(dayValue.AttemptedValue) && string.IsNullOrWhiteSpace(monthValue.AttemptedValue) && string.IsNullOrWhiteSpace(yearValue.AttemptedValue))
                    return null;

                int day;
                int month;
                int year;

                if (!int.TryParse(dayValue.AttemptedValue, out day) || !int.TryParse(monthValue.AttemptedValue, out month) || !int.TryParse(yearValue.AttemptedValue, out year))
                {
                    bindingContext.ModelState.AddModelError(name, $"{bindingContext.ModelMetadata.GetDisplayName()}: please supply a valid date");
                    return null;
                }

                var dateString = string.Format("{0:00}-{1:00}-{2:0000}", day, month, year);

                DateTime output;

                if (!DateTime.TryParseExact(dateString, "dd-MM-yyyy", null, DateTimeStyles.RoundtripKind, out output))
                    return null;

                return output;
            }
        }
    }
}