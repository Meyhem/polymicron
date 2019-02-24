using System;
using System.Collections.Generic;
using System.Text;

namespace Pm.Data.Entity
{
    public class Post
    {
        public int Id { get; set; }

        public string  Title { get; set; }

        public string Subtitle { get; set; }

        public string Body { get; set; }

        public bool Published { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public int Upvotes { get; set; }

        public int Downvotes { get; set; }

        public User Author { get; set; }

        public int? ThumbnailImageId { get; set; }

        public Image ThumbnailImage { get; set; }

        public ICollection<Rating> Ratings { get; set; }

        public ICollection<PostTag> Tags { get; set; }

        public ICollection<Comment> Comments { get; set; }
    }
}
