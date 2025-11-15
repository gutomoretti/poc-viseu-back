using PocViseu.Model.Config;
using PocViseu.Model.Extensions;
using PocViseu.Model.ModelView;

namespace PocViseu.Infrastructure.Querys
{
    public class WebcorpConfigExt
    {
        public static Func<WebcorpConfig, bool> Predicate(FilterNameView? q)
        {
            return x => (
                q.name!.Search(x.ParamKey!)
                );
        }
    }
}
