using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineBookShopping.Models
{
    public class BookModel
    {
        [Key]
        public int BookId { get; set; }

     //   [Required(ErrorMessage = "Please enter book title")]
        [Display(Name = "Book Title")]
        [StringLength(75)]
        public string BookTitle { get; set; }

      //  [Required(ErrorMessage = "Please enter book category")]
        [Display(Name = "Book Category")]
        [StringLength(75)]
        public string BookCategory { get; set; }

    //    [Required(ErrorMessage = "Please select book image")]
        [Display(Name = "Book Image")]
        [StringLength(75)]
        public string BookImage { get; set; }

      //  [Required(ErrorMessage = "Please add description.")]
        [StringLength(200)]
        public string Description { get; set; }

        //[Required]
        public int BookStock { get; set; }

      //  [Required]
        public int BookPrice { get; set; }

        public int? Quantity { get; set; }

       // [Required]
        public int TotalPage { get; set; }
        public int? AuthorId { get; set; }

        [ForeignKey("AuthorId")]
        public AuthorModel Author { get; set; }
        public int? PublisherId { get; set; }

        [ForeignKey("PublisherId")]
        public virtual PublisherModel Publisher { get; set; }

        public bool IsDeleted { get; set; }
        //[NotMapped]
        //[Required]
        //public IFormFile ProfileImage { get; set; }
    }
}
