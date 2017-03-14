using FormUI.Domain.BestStartGrantForms.Dto;

namespace FormUI.Controllers.Bsg
{
    public class RelationDetailsModel : NavigableModel
    {
        // GET
        public string   Title;
        public string   Heading;
        public bool     HideRelationship;

        // POST
        public RelationDetails RelationDetails;
    }
}