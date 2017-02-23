using System;

namespace FormUI.Domain.Util
{
    public class DomainRegistry
    {
        [ThreadStatic]
        public static IRepository Repository;

        [ThreadStatic]
        public static ValidationContext ValidationContext;

        public static Func<DateTime> NowUtc = () => DateTime.UtcNow;
    }
}