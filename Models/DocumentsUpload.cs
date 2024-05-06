using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using OnlineBookShopping.Areas.Identity.Pages.Account;

namespace OnlineBookShopping.Models
{
    public class DocumentsUpload
    {
        [Key]
        public int DocumentId { get; set; }

        [Required(ErrorMessage = "Please enter file name")]
        public string FileName { get; set; }
        [NotMapped]
        public IFormFile File { get; set; }
        public string UserId { get; set; }

    }
}
