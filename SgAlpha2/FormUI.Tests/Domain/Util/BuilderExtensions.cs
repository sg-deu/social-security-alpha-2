using FormUI.Domain.Util;

namespace FormUI.Tests.Domain.Util
{
    public static class BuilderExtensions
    {
        public static T Insert<T>(this Builder<T> builder)
        {
            return DomainRegistry.Repository.Insert(builder.Value());
        }
    }
}
