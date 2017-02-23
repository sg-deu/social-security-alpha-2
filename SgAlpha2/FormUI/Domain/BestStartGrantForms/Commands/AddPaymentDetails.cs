using FormUI.Domain.BestStartGrantForms.Dto;
using FormUI.Domain.Util;

namespace FormUI.Domain.BestStartGrantForms.Commands
{
    public class AddPaymentDetails : Command
    {
        public string           FormId;
        public PaymentDetails   PaymentDetails;

        public override void Execute()
        {
            var form = Repository.Load<BestStartGrant>(FormId);
            form.AddPaymentDetails(PaymentDetails);
        }
    }
}