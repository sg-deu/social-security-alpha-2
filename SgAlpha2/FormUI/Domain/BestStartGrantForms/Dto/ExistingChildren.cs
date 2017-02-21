using System.Collections.Generic;

namespace FormUI.Domain.BestStartGrantForms.Dto
{
    public class ExistingChildren
    {
        public ExistingChildren()
        {
            Children = new List<ExistingChild>();
        }

        public IList<ExistingChild> Children { get; set; }
    }
}