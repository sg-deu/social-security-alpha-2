using FormUI.Domain.Util;

namespace FormUI.Domain.BestStartGrantForms.Commands
{
    public class RemoveEvidenceFile : Command
    {
        public string FormId;
        public string CloudName;

        public override void Execute()
        {
            var form = Repository.Load<BestStartGrant>(FormId);
            form.RemoveEvidenceFile(CloudName);
        }
    }
}