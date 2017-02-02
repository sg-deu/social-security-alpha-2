using System;
using System.Collections.Generic;

namespace FormUI.Tests.Controllers.Util.Html
{
    public static class FormHelper
    {
        public static Func<ElementWrapper, FormScraper> NewScraper = ew => new FormScraper(ew);

        public static TypedForm<T> Scrape<T>(DocumentWrapper doc, string requestUrl)
        {
            return Scrape<T>(doc, requestUrl, "form");
        }

        public static TypedForm<T> Scrape<T>(DocumentWrapper doc, string requestUrl, int index)
        {
            var formElements = FindForms(doc, "form");

            if (index > formElements.Count - 1)
                throw new Exception(string.Format("Index '{0}' is too large for collection with '{1}' forms: {2}", index, formElements.Count, ElementWrapper.FormatTags(formElements)));

            return NewScraper(formElements[index]).Scrape<T>(requestUrl);
        }

        public static TypedForm<T> Scrape<T>(DocumentWrapper doc, string requestUrl, string cssSelector)
        {
            var formElements = FindForms(doc, cssSelector);

            if (formElements.Count > 1)
                throw new Exception("Multiple form elements found in document: " + ElementWrapper.FormatTags(formElements));

            return NewScraper(formElements[0]).Scrape<T>(requestUrl);
        }

        private static List<ElementWrapper> FindForms(DocumentWrapper doc, string cssSelector)
        {
            var formElements = doc.FindAll(cssSelector);

            if (formElements.Count == 0)
                throw new Exception(string.Format("CSS selector '{0}' did not match any elements in the document", cssSelector));

            return formElements;
        }
    }
}
