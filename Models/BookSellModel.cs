using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineBookShopping.Models
{
    public class BookSellModel
    {
        [Key]
        public int OrderId { get; set; }

        [Required]
        public string UserName { get; set; }

        public int? BookId { get; set; }
        [ForeignKey("BookId")]
        public virtual BookModel Book { get; set; }

        [Required]
        public string Payment { get; set; }

        [Required]
        public DateTime PurchaseDate { get; set; }

        [Required]
        public int Quantity { get; set; }

    }
}
