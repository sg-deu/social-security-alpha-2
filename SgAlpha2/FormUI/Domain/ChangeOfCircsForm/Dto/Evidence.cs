using System.Collections.Generic;
using System.ComponentModel;

namespace FormUI.Domain.ChangeOfCircsForm.Dto
{
    public class Evidence
    {
        [DisplayName("I confirm that I am sending documents by post")]
        public bool                 SendingByPost   { get; set; }

        public IList<EvidenceFile> Files = new List<EvidenceFile>();

        public void CopyTo(Evidence dest)
        {
            dest.SendingByPost = SendingByPost;
        }
    }
}