using FormUI.Controllers.Shared;
using FormUI.Domain.BestStartGrantForms.Dto;

namespace FormUI.Controllers.Bsg
{
    public class ExpectedChildrenModel : NavigableModel
    {
        public string TitlePrefix;
        public string Title;

        // POST
        public ExpectedChildren ExpectedChildren;
    }
}