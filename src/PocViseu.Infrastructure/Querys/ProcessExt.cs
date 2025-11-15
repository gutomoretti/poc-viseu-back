using PocViseu.Model.Bussines;
using PocViseu.Model.Extensions;
using PocViseu.Model.ModelView;

namespace PocViseu.Infrastructure.Querys
{
    public class ProcessExt
    {
        public static Func<Process, bool> Predicate(FilterProcessView? q)
        {
            //if (string.IsNullOrEmpty(q.name) && string.IsNullOrEmpty(q.DateInit)) return x => (true);

            return x => (
                q.name!.Search(x.HashId) &&
                q.empresa!.Search(x.empresa)
                // && q.DateInit.ParseDate2().SearchDataBetween(x.CreatedAt, q.DateEnd.ParseDate2End())
                );
        }
    }
}
