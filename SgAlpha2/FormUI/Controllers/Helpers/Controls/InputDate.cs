using System.Web.Mvc;
using HtmlTags;

namespace FormUI.Controllers.Helpers.Controls
{
    public class InputDate : InputControl
    {
        public InputDate(HtmlHelper helper, string id, string name) : base(helper, id, name)
        {
        }

        protected override HtmlTag CreateTag()
        {
            // need this to trigger the model binding for the property
            var hidden = new HtmlTag("input")
                .Attr("type", "hidden")
                .Name(Name);

            var day = CreateDatePartTag("Day", 2);
            var month = CreateDatePartTag("Month", 2);
            var year = CreateDatePartTag("Year", 4);

            var container = new HtmlTag("div")
                .AddClasses("date-input")
                .Append(hidden)
                .Append(day)
                .Append(month)
                .Append(year);

            return container;
        }

        private HtmlTag CreateDatePartTag(string part, int maxLength)
        {
            var id = Id + "_" + part.ToLower();
            var name = Name + "_" + part.ToLower();

            var label = new HtmlTag("label").Attr("for", id).Text(part);

            var input = new HtmlTag("input")
                .Id(id)
                .Name(name)
                .Attr("type", "text")
                .Attr("maxlength", maxLength);

            var partContainer = new HtmlTag("div")
                .AddClasses(part.ToLower())
                .Append(label)
                .Append(input);

            return partContainer;
        }
    }
}