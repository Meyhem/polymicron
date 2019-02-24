using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Pm.Data.Entity;
using System.Collections.Generic;
using System.Linq;

namespace Pm.Data.Extensions
{
    public static class IPostQueryableExtensions
    {
        public static IQueryable<Post> WithTags(this IQueryable<Post> self)
        {
            return self.Include(i => i.Tags);
        }

        public static IQueryable<Post> WithThumbnail(this IQueryable<Post> self)
        {
            return self.Include(i => i.ThumbnailImage);
        }

        public static IQueryable<Post> WithAuthor(this IQueryable<Post> self)
        {
            return self.Include(i => i.Author);
        }

        public static IQueryable<Post> WithComments(this IQueryable<Post> self)
        {
            return self.Include(i => i.Comments);
        }

        public static IQueryable<Post> SearchFilter(this IQueryable<Post> self, string search)
        {
            
            return self.Where(p => EF.Functions.ILike(p.Title, $"%{search}%") ||
                EF.Functions.ILike(p.Subtitle, $"%{search}%") ||
                p.Tags.Any(t => EF.Functions.ILike(t.Tag, $"%{search}%")));
        }
    }
}
