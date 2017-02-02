using System;
using System.Collections.Generic;
using System.Linq;
using AngleSharp.Dom;

namespace FormUI.Tests.Controllers.Util.Html
{
    public class ElementWrapper : ParentNodeWrapper
    {
        private IElement _element;

        public ElementWrapper(IElement element) : base(element)
        {
            _element = element;
        }

        public IElement Element     { get { return _element; } }
        public string   ClassName   { get { return Element.ClassName; } }
        public string   Id          { get { return Element.Id; } }
        public string   InnerHtml   { get { return Element.InnerHtml; } }
        public string   OuterHtml   { get { return Element.OuterHtml; } }
        public string   TagName     { get { return Element.TagName; } }
        public string   TextContent { get { return Element.TextContent; } }

        public string Attribute(string name)
        {
            var attribute = _element.Attributes[name];

            if (attribute == null)
                throw new Exception(string.Format("Could not find attribute '{0}' on element {1}", name, FormatTag()));

            return attribute.Value;
        }

        public bool HasAttribute(string name)
        {
            return _element.HasAttribute(name);
        }

        public string AttributeOrEmpty(string name)
        {
            return HasAttribute(name) ? Attribute(name) : "";
        }

        public ElementWrapper Where(Action<ElementWrapper> verification)
        {
            verification(this);
            return this;
        }

        public string FormatTag()
        {
            var html = _element.OuterHtml;
            var firstTagClose = html.IndexOf('>');
            return html.Substring(0, firstTagClose + 1);
        }

        public static string FormatTags(IEnumerable<ElementWrapper> elements)
        {
            return string.Join("\n", elements.Select(e => e.FormatTag()));
        }
    }
}
