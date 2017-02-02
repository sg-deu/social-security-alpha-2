using FormUI.Tests.Controllers.Util.Http;

namespace FormUI.Tests.Controllers.Util.Html
{
    public class SubmitValue
    {
        private string _name;
        private string _value;

        public SubmitValue(string name = "", string value = "Submit")
        {
            _name = name;
            _value = value;
        }

        public string   Name    { get { return _name; } }
        public string   Value   { get { return _value; } }

        public static SubmitValue FromElement(ElementWrapper inputSubmit)
        {
            if (!inputSubmit.HasAttribute("name"))
                return new SubmitValue();

            var name = inputSubmit.Attribute("name");

            if (!inputSubmit.HasAttribute("value"))
                return new SubmitValue(name);

            var value = inputSubmit.Attribute("value");

            return new SubmitValue(name, value);
        }

        public virtual void SetFormValue(Request request)
        {
            if (!string.IsNullOrEmpty(_name))
                request.AddFormValue(_name, _value);
        }
    }
}