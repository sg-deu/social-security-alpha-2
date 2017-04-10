using FormUI.Domain.Util;

namespace FormUI.Domain.BestStartGrantForms.Commands
{
    public class AddEvidenceFile : Command
    {
        public string FormId;
        public string Filename;
        public byte[] Content;

        public override void Execute()
        {
            var form = Repository.Load<BestStartGrant>(FormId);
            form.AddEvidenceFile(Filename, Content);
        }
    }
}