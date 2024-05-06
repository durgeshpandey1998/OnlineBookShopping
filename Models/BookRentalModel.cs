using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineBookShopping.Models
{
    public class BookRentalModel
    {
        [Key]
        public int BookRentId { get; set; }
        [Required]
        public string UserName { get; set; }

        public int? BookId { get; set; }
        [ForeignKey("BookId")]
        public virtual BookModel Book { get; set; }

        [Required]
        public DateTime BookRequestDate { get; set; }
        public DateTime? IssueDate { get; set; }
        public DateTime? ReturnDate { get; set; }

        public int Fine { get; set; }

        public bool IsIssued { get; set; }
        public bool IsReturn { get; set; }

        public int? DelayDay { get; set; }

    }
}
