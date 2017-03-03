using System;
using System.Linq.Expressions;

namespace FormUI.Controllers.Helpers
{
    public class AjaxAction
    {
        public string   Action      { get; protected set; }
        public string   TargetId    { get; protected set; }
        public bool     Show        { get; protected set; }

        private AjaxAction() { }

        public static AjaxAction ShowHideFormGroup<T>(Expression<Func<T, object>> property, bool show)
        {
            return new AjaxAction
            {
                Action = "ShowHide",
                TargetId = property.GetExpressionText() + "_FormGroup",
                Show = show,
            };
        }
    }
}