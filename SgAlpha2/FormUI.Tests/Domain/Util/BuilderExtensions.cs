using System;
using FormUI.Domain.Util;

namespace FormUI.Tests.Domain.Util
{
    public static class BuilderExtensions
    {
        public static T Insert<T>(this Builder<T> builder, Action<T> mutator = null)
        {
            return DomainRegistry.Repository.Insert(builder.Value(mutator));
        }
    }
}
