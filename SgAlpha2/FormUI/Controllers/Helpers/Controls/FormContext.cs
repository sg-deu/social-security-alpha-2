using System.Web.Mvc;

namespace FormUI.Controllers.Helpers.Controls
{
    public class ControlContext
    {
        public HtmlHelper       Helper;
        public ModelMetadata    Metadata;
        public string           Id;
        public string           Name;
    }
}