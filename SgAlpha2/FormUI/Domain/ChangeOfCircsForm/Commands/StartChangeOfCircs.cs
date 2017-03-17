using FormUI.Domain.ChangeOfCircsForm.Responses;
using FormUI.Domain.Util;

namespace FormUI.Domain.ChangeOfCircsForm.Commands
{
    public class StartChangeOfCircs : Command<NextSection>
    {
        public override NextSection Execute()
        {
            return ChangeOfCircs.Start();
        }
    }
}