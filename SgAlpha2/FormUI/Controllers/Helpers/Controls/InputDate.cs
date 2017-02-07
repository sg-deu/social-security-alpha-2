using HtmlTags;

namespace FormUI.Controllers.Helpers.Controls
{
    public class InputDate : InputControl
    {
        public InputDate(string id, string name) : base(id, name)
        {
        }

        protected override HtmlTag CreateTag()
        {
            var day = CreateDatePartTag("Day", 2);
            var month = CreateDatePartTag("Month", 2);
            var year = CreateDatePartTag("Year", 4);

            var container = new HtmlTag("div")
                .AddClasses("date-input")
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
                .Attr("id", id)
                .Attr("name", name)
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