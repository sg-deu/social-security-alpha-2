using FormUI.Domain.Util;

namespace FormUI.Domain.ChangeOfCircsForm.Commands
{
    public class AddEvidenceFile : Command
    {
        public string   FormId;
        public string   Filename;
        public byte[]   Content;

        public override void Execute()
        {
            var form = Repository.Load<ChangeOfCircs>(FormId);
            form.AddEvidenceFile(Filename, Content);
        }
    }
}