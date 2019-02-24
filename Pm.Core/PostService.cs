using Microsoft.EntityFrameworkCore;
using Pm.Common.Exceptions;
using Pm.Common.Extensions;
using Pm.Data.Entity;
using Pm.Data.Extensions;
using Pm.Data.Interfaces;
using Pm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pm.Core
{
    public class PostService
    {
        private const int PostListPageSize = 10;
        private const int PublicPostListPageSize = 8;

        private readonly IRepository<Post> postRepo;
        private readonly IRepository<User> userRepo;
        private readonly IRepository<PostTag> postTagsRepo;
        private readonly IRepository<Image> imageRepo;
        private readonly IRepository<Rating> ratingRepo;

        public PostService(IRepository<Post> postRepo, 
            IRepository<User> userRepo, 
            IRepository<PostTag> postTagsRepo,
            IRepository<Image> imageRepo,
            IRepository<Rating> ratingRepo,
            IRepository<Comment> commentRepo)
        {
            this.postRepo = postRepo;
            this.userRepo = userRepo;
            this.postTagsRepo = postTagsRepo;
            this.imageRepo = imageRepo;
            this.ratingRepo = ratingRepo;
        }

        public async Task<PostModel> FetchEditablePost(int id)
        {
            var post = await postRepo.Query()
                .Include(i => i.Tags)
                .SingleOrDefaultAsync(p => p.Id == id);

            PmAssert.AssertOrNotFound(post != null, "Post not found");


            return PostModel.FromPost(post);
        }

        public async Task<ThumbnailModel> FetchThumbnail(int id)
        {
            var post = await postRepo.Query()
                .Include(i => i.ThumbnailImage)
                .SingleOrDefaultAsync(p => p.Id == id);

            PmAssert.AssertOrNotFound(post != null, "Post not found");
            PmAssert.AssertOrNotFound(post.ThumbnailImageId != null, "Post has no thumbnail");


            return new ThumbnailModel
            {
                Id = post.ThumbnailImageId,
                Data = post.ThumbnailImage?.Data,
                Mime = post.ThumbnailImage?.Mime
            };
        }

        public async Task<PostListModel> FetchPublicPosts(int page, string search = null)
        {
            var posts = postRepo.Query()
                .WithTags()
                .OrderByDescending(p => p.CreatedAt)
                .Where(p => p.Published);

            if (!string.IsNullOrWhiteSpace(search))
            {
                posts = posts.SearchFilter(search);
            }

            // paralelize
            var totalCountTask = posts.CountAsync();
            posts = posts.Paginate(page, PublicPostListPageSize);
            var postsTask = posts.ToListAsync();
            var postsCountTask = posts.CountAsync();

            // synchronize
            await Task.WhenAll(totalCountTask, postsTask, postsCountTask);

            // materialize
            var matTotal = await totalCountTask;
            var matPosts = await postsTask;
            var matPostsCount = await postsCountTask;

            return new PostListModel
            {
                Page = page,
                Pages = (int)Math.Ceiling(matTotal / (double)PublicPostListPageSize),
                Search = search,
                Posts = matPosts.Select(PostModel.FromPost)
            };
        }

        public async Task<PostModel> FetchOnePublicPost(int id, string uvi)
        {
            var post = await postRepo.Query()
                .WithAuthor()
                .WithTags()
                .WithComments()
                .SingleOrDefaultAsync(p => p.Id == id && p.Published);

            PmAssert.AssertOrNotFound(post != null, "Post not found");

            var model = PostModel.FromPost(post);

            if (uvi != null)
            {
                model.CanVote = true;

                var rating = await ratingRepo.Query()
                    .SingleOrDefaultAsync(r => r.PostId == id && r.Token == uvi);

                if (rating != null)
                {
                    model.Upvoted = rating.Direction;
                    model.Downvoted = !rating.Direction;
                }
            }

            return model;
        }

        public async Task<PostListModel> FetchAll(int page)
        {
            var countTask = postRepo.Query()
                .CountAsync();

            var postsTask = postRepo.Query()
                .OrderByDescending(p => p.CreatedAt)
                .Skip((page - 1) * PostListPageSize)
                .Take(10)
                .Select(p => new Post
                {
                    Id = p.Id,
                    Title = p.Title,
                    Subtitle = p.Subtitle,
                    ThumbnailImageId = p.ThumbnailImageId,
                    Author = p.Author,
                    UpdatedAt = p.UpdatedAt,
                    CreatedAt = p.CreatedAt,
                    Published = p.Published
                })
                .ToListAsync();

            var model = new PostListModel();

            await Task.WhenAll(countTask, postsTask);

            var count = await countTask;
            var posts = await postsTask;

            model.Page = page;
            model.Pages = (int)Math.Ceiling(count / (double)PostListPageSize);
            model.Posts = posts.Select(PostModel.FromPost);

            return model;
        }

        public async Task Vote(int id, string direction, string uvi)
        {
            if (uvi == null) return;

            var dir = direction == "up";

            var rating = await ratingRepo.Query()
                .SingleOrDefaultAsync(r => r.Post.Id == id && r.Token == uvi);

            var post =  await postRepo.FindOne(id);

            // not rated before
            if (rating == null)
            {
                await ratingRepo.Create(new Rating
                {
                    Direction = dir,
                    Token = uvi,
                    PostId = post.Id
                });

                if (dir)
                {
                    post.Upvotes++;
                }
                else
                {
                    post.Downvotes++;
                }
            }
            // rated before
            else
            {
                // vote is same as previous, remove it
                if (dir == rating.Direction)
                {
                    await ratingRepo.Delete(rating);
                    if (dir)
                    {
                        post.Upvotes--;
                    }
                    else
                    {
                        post.Downvotes--;
                    }
                }
                else
                {
                    rating.Direction = dir;

                    // change up
                    if (dir)
                    {
                        post.Upvotes++;
                        post.Downvotes--;
                    }
                    // change down
                    else
                    {
                        post.Upvotes--;
                        post.Downvotes++;
                    }
                }
            }

            if (post.Upvotes < 0) post.Upvotes = 0;
            if (post.Downvotes < 0) post.Downvotes = 0;

            await postRepo.Update(post);
        }

        public async Task<int> SavePost(PostModel model, int userId)
        {
            Post post;
            var now = DateTime.Now;

            if (model.Id.HasValue)
            {
                post = await postRepo.Query()
                    .Include(i => i.Tags)
                    .SingleOrDefaultAsync(p => p.Id == model.Id);

                PmAssert.AssertOrNotFound(post != null, "Post not found");
            }
            else
            {
                post = new Post
                {
                    CreatedAt = now
                };
            }

            post.Title = model.Title;
            post.Subtitle = model.Subtitle;
            post.Body = model.Body;
            post.UpdatedAt = now;
            post.Author = post.Author ?? await userRepo.FindOne(userId);

            var tags = model.TagsAggr?.Split(",");
            var tagEnts = new List<PostTag>();
            if (tags != null)
            {
                tagEnts.AddRange(tags.Select(t => new PostTag
                {
                    Tag = t,
                }));
            }
            post.Tags = tagEnts;

            if (model.Id.HasValue)
            {
                await postRepo.Update(post); 
            }
            else
            {
                await postRepo.Create(post);
            }

            return post.Id;
        }

        public async Task SetPublished(int id, bool published)
        {
            var post = await postRepo.FindOne(id);

            PmAssert.AssertOrNotFound(post != null, "Post not found");

            post.Published = published;

            await postRepo.Update(post);
        }

        public async Task DeletePost(int id)
        {
            await postRepo.Delete(new Post { Id = id });
        }

        public async Task SaveThumbnail(int postId, byte[] data, string contentType)
        {
            var post = await postRepo.Query()
                .Include(i => i.ThumbnailImage)
                .SingleOrDefaultAsync(p => p.Id == postId);

            PmAssert.AssertOrNotFound(post != null, "Post not found");

            var imageId = post.ThumbnailImageId;

            if (imageId.HasValue)
            {
                await imageRepo.Delete(post.ThumbnailImage);
            }

            if (data != null)
            {
                post.ThumbnailImage = new Image
                {
                    Data = data,
                    Mime = contentType
                };
            }
            else
            {
                post.ThumbnailImage = null;
            }

            await postRepo.Update(post);
        }

        public async Task AddComment(int postId, CommentModel model)
        {
            var comment = new Comment
            {
                Name = model.Name,
                Text = model.Text,
                CreatedAt = DateTime.Now
            };

            var post = await postRepo.FindOne(postId);

            PmAssert.AssertOrNotFound(post != null, "Post not found");

            post.Comments = post.Comments ?? new List<Comment>();
            
            post.Comments.Add(comment);

            await postRepo.Update(post);
        }

    }
}
