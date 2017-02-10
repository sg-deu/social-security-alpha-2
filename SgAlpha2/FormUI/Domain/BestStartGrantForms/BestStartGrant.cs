using System;

namespace FormUI.Domain.BestStartGrantForms
{
    public class BestStartGrant : Forms.Form
    {
        public BestStartGrant() : base(Guid.NewGuid().ToString())
        {
        }

        public string Value { get; set; }
    }
}