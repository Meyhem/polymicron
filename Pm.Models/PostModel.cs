using Pm.Common.Extensions;
using Pm.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pm.Models
{
    public class PostModel
    {
        public int? Id { get; set; }

        public string Title { get; set; }

        public string TitleSlug { get; set; }

        public string Subtitle { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public string TagsAggr { get; set; }

        public IEnumerable<TagModel> Tags { get; set; }

        public string Body { get; set; }

        public bool Published { get; set; }

        public int Upvotes { get; set; }

        public int Downvotes { get; set; }

        public bool CanVote { get; set; }

        public bool Upvoted { get; set; }

        public bool Downvoted { get; set; }

        public int? ThumbnailId { get; set; }

        public byte[] Thumbnail { get; set; }

        public bool HasThumbnail => ThumbnailId != null;

        public UserModel Author { get; set; }

        public string ReCaptchaKey { get; set; }

        public IEnumerable<CommentModel> Comments { get; set; }

        public static PostModel FromPost(Post post) => new PostModel
        {
            Id = post.Id,
            Title = post.Title,
            TitleSlug = post.Title?.Slug(),
            Subtitle = post.Subtitle,
            CreatedAt = post.CreatedAt,
            UpdatedAt = post.UpdatedAt,
            Body = post.Body,
            Published = post.Published,
            Upvotes = post.Upvotes,
            Downvotes = post.Downvotes,
            TagsAggr = string.Join(",", post.Tags?.Select(t => t.Tag) ?? new string[0]),
            Tags = post.Tags?.Select(TagModel.FromPostTag),
            Thumbnail = post.ThumbnailImage?.Data,
            ThumbnailId = post.ThumbnailImageId,
            Author = UserModel.FromUser(post.Author),
            Comments = post.Comments?.Select(CommentModel.FromComment)
        };
    }
}
