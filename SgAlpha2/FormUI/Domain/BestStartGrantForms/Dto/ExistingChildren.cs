using System.Collections.Generic;
using System.ComponentModel;

namespace FormUI.Domain.BestStartGrantForms.Dto
{
    public class ExistingChildren
    {
        public ExistingChildren()
        {
            Children = new List<ExistingChild>();
        }

        [DisplayName("Do you have any children you're responsible for in your household?")]
        public bool?                AnyExistingChildren { get; set; }

        public IList<ExistingChild> Children            { get; set; }
    }
}