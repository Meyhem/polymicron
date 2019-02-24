using System;
using System.Collections.Generic;
using System.Text;

namespace Pm.Data.Entity
{
    public class Image
    {
        public int Id { get; set; }

        public byte[] Data { get; set; }

        public string Mime { get; set; }

        public Post Post { get; set; }
    }
}
