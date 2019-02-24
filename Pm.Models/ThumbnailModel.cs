using System;
using System.Collections.Generic;
using System.Text;

namespace Pm.Models
{
    public class ThumbnailModel
    {
        public int? Id { get; set; }

        public string Mime { get; set; }

        public byte[] Data { get; set; }
    }
}
