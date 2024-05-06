using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineBookShopping.Models
{
    public class AuthorModel
    {
        [Key]
        public int AuhtorId { get; set; }
        public string AuthorName { get; set; }
       
    }
}
