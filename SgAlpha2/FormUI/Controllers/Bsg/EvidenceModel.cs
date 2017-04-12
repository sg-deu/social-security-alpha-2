using System.Collections.Generic;
using FormUI.Controllers.Shared;
using FormUI.Domain.BestStartGrantForms.Dto;

namespace FormUI.Controllers.Bsg
{
    public class EvidenceModel : NavigableModel
    {
        // GET
        public IList<string> UploadedFiles;

        // POST
        public Evidence Evidence;
    }
}