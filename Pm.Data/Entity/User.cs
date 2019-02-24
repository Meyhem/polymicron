using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Pm.Data.Entity
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string DisplayName{ get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public ICollection<Post> Posts { get; set; }
    }
}
