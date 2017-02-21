using System.Linq;

namespace FormUI.Domain.Util
{
    public interface IRepository
    {
        T                       Insert<T>(T doc);
        T                       Load<T>(string id);
        T                       Update<T>(T doc) where T : IDocument;
        void                    Delete<T>(T doc) where T : IDocument;

        IOrderedQueryable<T>    Query<T>();
    }
}