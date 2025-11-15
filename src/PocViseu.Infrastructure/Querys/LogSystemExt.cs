using PocViseu.Model.Bussines;
using PocViseu.Model.Extensions;
using PocViseu.Model.ModelView;

namespace PocViseu.Infrastructure.Querys
{
    public static class LogSystemExt
    {
        public static Func<LogSystem, bool> Predicate(FilterLogSystemView? q)
        {
            if (string.IsNullOrEmpty(q.name))
            {
                return x => (
                     q.LevelId.Search(x.Level)
               );
            }

            return x => (
                q.LevelId.Search(x.Level) &&
                q.name.SearchTraceKey(x.TraceKey)
                //q.DateInit.ParseDate2().SearchData(x.CreatedAt)
                //x.CreatedAt.SearchDataBetween(q.DateInit.ParseDate2(),q.DateEnd.ParseDate2())
                //q.DateInit.ParseDate2().SearchDataBetween(x.CreatedAt, q.DateEnd.ParseDate2End())
                //&& x.CreatedAt.
                );
        }
    }
}
