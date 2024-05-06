using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineBookShopping.Models
{
    [Keyless]
    public class PassRecovery
    {
        [EmailAddress,Display(Name ="Email Address")]
        public string Email { get; set; }
        public bool EmailSent { get; set; }
    }
}
