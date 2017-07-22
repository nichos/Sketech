using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sketech.Entities.Identity
{
    public class UsersInRoles
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
    }
}
