using FormUI.Domain.ChangeOfCircsForm.Dto;
using FormUI.Domain.ChangeOfCircsForm.Responses;
using FormUI.Domain.Util;

namespace FormUI.Domain.ChangeOfCircsForm.Commands
{
    public class AddPaymentDetails : Command<NextSection>
    {
        public string           FormId;
        public PaymentDetails   PaymentDetails;

        public override NextSection Execute()
        {
            var form = Repository.Load<ChangeOfCircs>(FormId);
            return form.AddPaymentDetails(PaymentDetails);
        }
    }
}