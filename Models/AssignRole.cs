using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineBookShopping.Models
{
    [Keyless]
    public class AssignRole
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }
    }
}
