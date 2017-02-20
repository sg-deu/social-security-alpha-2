using FormUI.Domain.Util;
using System.Diagnostics.CodeAnalysis;

namespace FormUI.Tests.Domain.Util
{
    [ExcludeFromCodeCoverage]
    public static class BuilderExtensions
    {
        public static T Insert<T>(this Builder<T> builder)
        {
            return DomainRegistry.Repository.Insert(builder.Value());
        }
    }
}
