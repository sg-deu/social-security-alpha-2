using FormUI.Domain.ChangeOfCircsForm.Dto;

namespace FormUI.Domain.ChangeOfCircsForm.Responses
{
    public class CocDetail
    {
        public string Id;

        public Consent      Consent;

        public Sections?    PreviousSection;
        public bool         IsFinalSection;
    }
}