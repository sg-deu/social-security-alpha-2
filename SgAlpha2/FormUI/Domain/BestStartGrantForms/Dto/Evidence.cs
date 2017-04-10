using System.Collections.Generic;
using System.ComponentModel;

namespace FormUI.Domain.BestStartGrantForms.Dto
{
    public class Evidence
    {
        [DisplayName("I confirm I’m posting the document(s) to the Social Security Agency address")]
        public bool SendingByPost { get; set; }

        public IList<EvidenceFile> Files = new List<EvidenceFile>();

        public void CopyTo(Evidence dest)
        {
            dest.SendingByPost = SendingByPost;
        }
    }
}