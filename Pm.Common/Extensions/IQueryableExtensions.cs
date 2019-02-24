using System.Linq;

namespace Pm.Common.Extensions
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> Paginate<T>(this IQueryable<T> self, int page, int pageSize)
        {
            return self.Skip((page - 1) / pageSize)
                .Take(pageSize);
        }
    }
}
