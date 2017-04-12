using FormUI.Controllers.Shared;
using FormUI.Domain.BestStartGrantForms.Dto;

namespace FormUI.Controllers.Bsg
{
    public class BenefitsModel : NavigableModel
    {
        // GET
        public string   Title;
        public string   Question;

        // POST
        public Benefits Benefits;
    }
}