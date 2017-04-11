using FormUI.Domain.Util;

namespace FormUI.Domain.ChangeOfCircsForm.Commands
{
    public class RemoveEvidenceFile : Command
    {
        public string FormId;
        public string CloudName;

        public override void Execute()
        {
            var form = Repository.Load<ChangeOfCircs>(FormId);
            form.RemoveEvidenceFile(CloudName);
        }
    }
}