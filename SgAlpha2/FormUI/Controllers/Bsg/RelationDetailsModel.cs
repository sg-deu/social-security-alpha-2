using FormUI.Controllers.Shared;
using FormUI.Domain.BestStartGrantForms.Dto;
using FormUI.Domain.Forms.Dto;

namespace FormUI.Controllers.Bsg
{
    public class RelationDetailsModel : NavigableModel
    {
        // GET
        public string   Title;
        public string   Heading;
        public bool     HideRelationship;
        public Address  InheritedAddress;

        // POST
        public RelationDetails RelationDetails;
    }
}