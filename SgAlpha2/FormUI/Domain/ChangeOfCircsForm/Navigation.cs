using System;
using System.Collections.Generic;
using System.Linq;

namespace FormUI.Domain.ChangeOfCircsForm
{
    public static class Navigation
    {
        private static readonly IList<Sections> _order;

        static Navigation()
        {
            _order = Enum.GetValues(typeof(Sections)).Cast<Sections>().ToList();
        }

        public static IEnumerable<Sections> Order { get { return _order; } }
    }
}