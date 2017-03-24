using System.Collections.Generic;
using FormUI.Domain.ChangeOfCircsForm.Dto;

namespace FormUI.Controllers.Coc
{
    public class EvidenceModel : NavigableModel
    {
        // GET
        public IList<string> UploadedFiles;

        // POST
        public Evidence Evidence;
    }
}