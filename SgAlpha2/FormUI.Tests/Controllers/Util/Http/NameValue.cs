using System.Web;

namespace FormUI.Tests.Controllers.Util.Http
{
    public class NameValue
    {
        private string  _name;
        private string  _value;

        public NameValue(string name, string value)
        {
            _name = name;
            _value = value;
        }

        public string   Name    { get { return _name; } }
        public string   Value   { get { return _value; } }

        public string UrlQueryValue()
        {
            var name = HttpUtility.UrlEncode(Name);
            var value = HttpUtility.UrlEncode(Value);
            return string.Format("{0}={1}", name, value);
        }
    }
}
