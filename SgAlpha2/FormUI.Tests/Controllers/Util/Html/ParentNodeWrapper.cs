using System;
using System.Collections.Generic;
using System.Linq;
using AngleSharp.Dom;

namespace FormUI.Tests.Controllers.Util.Html
{
    public abstract class ParentNodeWrapper
    {
        private IParentNode _parentNode;

        public ParentNodeWrapper(IParentNode parentNode)
        {
            _parentNode = parentNode;
        }

        public ElementWrapper Find(string cssSelector)
        {
            var elements = _parentNode.QuerySelectorAll(cssSelector);

            if (elements.Length == 0)
                throw new Exception(string.Format("Could not find any element matching selector '{0}'", cssSelector));

            if (elements.Length > 1)
                throw new Exception(string.Format("Expected 1 element, but found {0} elements matching selector '{1}'", elements.Length, cssSelector));

            return new ElementWrapper(elements[0]);
        }

        public List<ElementWrapper> FindAll(string cssSelector)
        {
            var elementList = _parentNode.QuerySelectorAll(cssSelector);
            var elementWrapperList = elementList.Select(e => new ElementWrapper(e)).ToList();
            return elementWrapperList;
        }
    }
}
