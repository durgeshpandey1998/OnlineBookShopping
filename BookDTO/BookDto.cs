using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using OnlineBookShopping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineBookShopping.BookDTO
{
    public class BookDto
    {
        public List<BookModel> book { get; set; }
        public List<AuthorModel> Author { get; set; }
        public AuthorModel AuthorProperty { get; set; }
        public List<PublisherModel> Publisher { get; set; }
        public PublisherModel PublisherProperty { get; set; }
        public BookModel BookProperty { get; set; }
        public List<BookSellModel> BookSell { get; set; }
        public BookSellModel BookSellProperty { get; set; }
        public BookRentalModel BookRentalProperty { get; set; }
        public List<BookRentalModel> BookRentals { get; set; }


    }
}
