using PocViseu.Model.Auth;
using PocViseu.Model.Extensions;
using PocViseu.Model.ModelView;

namespace PocViseu.Infrastructure.Querys
{
    public class UserExt
    {
        public static Func<User, bool> Predicate(FilterNameView? q)
        {
            return x => (
                q.name!.Search(x.Username!) || q.name!.Search(x.Email!)
                );
        }
    }
}
