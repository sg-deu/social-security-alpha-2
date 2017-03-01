using System;
using System.Web.Mvc;

namespace FormUI.Controllers.Helpers
{
    public class ScopedHtmlHelper<T> : IDisposable
    {
        private HtmlHelper<T>   _html;
        private Action          _onDispose;

        public HtmlHelper<T> Html { get { return _html; } }

        public ScopedHtmlHelper(HtmlHelper<T> html) : this(html, null) { }

        public ScopedHtmlHelper(HtmlHelper<T> html, Action onDispose)
        {
            _html = html;
            _onDispose = onDispose ?? (() => { });
        }

        public void Dispose()
        {
            _onDispose();
        }
    }
}