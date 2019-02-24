using Microsoft.EntityFrameworkCore;
using Pm.Data.Entity;

namespace Pm.Data
{
    public class PmEntities: DbContext
    {
        public PmEntities(DbContextOptions<PmEntities> opts): base(opts) { }

        public DbSet<User> Users { get; set; }

        public DbSet<Post> Posts { get; set; }

        public DbSet<Rating> Ratings { get; set; }

        public DbSet<PostTag> PostTags { get; set; }

        public DbSet<Image> Images { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            builder.Entity<User>()
                .HasMany(u => u.Posts)
                .WithOne(p => p.Author);

            builder.Entity<Post>()
                .HasMany(p => p.Ratings)
                .WithOne(r => r.Post)
                .HasForeignKey(r => r.PostId);

            builder.Entity<Post>()
                .HasMany(p => p.Tags)
                .WithOne(t => t.Post)
                .HasForeignKey(pt => pt.PostId);

            builder.Entity<Post>()
                .HasOne(p => p.ThumbnailImage)
                .WithOne(i => i.Post)
                .HasForeignKey<Post>(p => p.ThumbnailImageId)
                .IsRequired(false);

            builder.Entity<Post>()
                .HasMany(p => p.Comments)
                .WithOne(c => c.Post);

            builder.Entity<PostTag>()
                .HasIndex(pt => pt.Tag);

            builder.Entity<Rating>()
                .HasIndex(r => new { r.PostId, r.Token })
                .IsUnique();

            base.OnModelCreating(builder);
        }
    }
}
