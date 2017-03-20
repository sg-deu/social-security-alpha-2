using FormUI.Domain.ChangeOfCircsForm.Responses;
using FormUI.Domain.Util;

namespace FormUI.Domain.ChangeOfCircsForm.Queries
{
    public class FindCocSection : Query<CocDetail>
    {
        public string   FormId;
        public Sections Section;

        public override CocDetail Find()
        {
            var form = Repository.Load<ChangeOfCircs>(FormId);
            return form.FindSection(Section);
        }
    }
}