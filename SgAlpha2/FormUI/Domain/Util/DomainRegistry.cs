using System;

namespace FormUI.Domain.Util
{
    public class DomainRegistry
    {
        [ThreadStatic]
        public static IRepository Repository;
    }
}