using Pm.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Pm.Models
{
    public class CommentModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Text { get; set; }

        [Required]
        [MaxLength(20)]
        public string Name { get; set; }

        public DateTime CreatedAt { get; set; }

        public static CommentModel FromComment(Comment self)
        {
            return new CommentModel
            {
                Id = self.Id,
                CreatedAt = self.CreatedAt,
                Name = self.Name,
                Text = self.Text
            };
        }
    }
}
