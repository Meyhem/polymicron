using System;
using System.Collections.Generic;
using System.Text;

namespace Pm.Data.Entity
{
    public class Comment
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public string Name { get; set; }

        public DateTime CreatedAt { get; set; }

        public Post Post { get; set; }
    }
}
