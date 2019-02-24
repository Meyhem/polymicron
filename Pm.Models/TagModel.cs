using Pm.Data.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pm.Models
{
    public class TagModel
    {
        public int Id { get; set; }

        public string Tag { get; set; }

        public static TagModel FromPostTag(PostTag pt)
        {
            return new TagModel
            {
                Id = pt.Id,
                Tag = pt.Tag
            };
        }
    }
}
