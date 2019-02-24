using Pm.Data.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pm.Models
{
    public class UserModel
    {
        public string Username { get; set; }

        public string DisplayName { get; set; }

        public static UserModel FromUser(User user)
        {
            if (user == null) return null;

            return new UserModel
            {
                Username = user.Username,
                DisplayName = user.DisplayName
            };
        }
    }
}
