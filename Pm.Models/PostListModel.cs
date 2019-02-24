using System;
using System.Collections.Generic;
using System.Text;

namespace Pm.Models
{
    public class PostListModel
    {
        public int Page { get; set; }

        public int Pages { get; set; }

        public string Search { get; set; }

        public IEnumerable<PostModel> Posts { get; set; }
    }
}
