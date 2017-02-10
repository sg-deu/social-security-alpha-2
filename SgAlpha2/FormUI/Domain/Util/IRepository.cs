using System.Linq;

namespace FormUI.Domain.Util
{
    public interface IRepository
    {
        void                    Insert<T>(T doc);
        T                       Load<T>(string id);
        void                    Update<T>(T doc) where T : IDocument;

        IOrderedQueryable<T>    Query<T>();
    }
}