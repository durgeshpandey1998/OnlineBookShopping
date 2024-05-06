using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineBookShopping.Models;
using OnlineBookShopping.BookDTO;

namespace OnlineBookShopping.Interfaces
{
    public interface IBook
    {
        List<BookModel> GetAllBook();
        BookModel GetBook(int id);
        //BookModel Create(BookModel book);
        BookDto Edit(BookDto BookData);
        //BookModel Delete(BookModel book);
        List<AuthorModel> GetAllAuthor();
        List<PublisherModel> GetAllPublisher();
        BookDto AddBook(BookDto BookData);
        BookModel GetOneBook(int id);
        BookModel Delete(int id);
        AuthorModel AddAuthor(AuthorModel AuthorData);
        PublisherModel AddPublisher(PublisherModel PublisherData);
        
        //Order Book Method List
        BookSellModel OrderBook(BookSellModel BookSellData);
        BookModel UpdateStock(BookModel BookData);
        List<BookSellModel> GetAllOrder();
        List<BookSellModel> OrderListForUser(string username);

        //Book Rental Method List
        BookRentalModel BookRent(BookRentalModel BookRequest);
        List<BookRentalModel> GetAllRentalBook();
        List<BookRentalModel> GetAllRentalBookByUser(string username);

        BookRentalModel BookIssue(BookRentalModel BookIssue);
        BookRentalModel BookReturn(BookRentalModel BookReturn);
        BookModel UpDateBookStock(BookModel BookData);
        BookModel ReturnUpDateBookStock(BookModel BookData);

        BookModel AddBookAPI(BookModel BookData);
        BookModel EditAPI(BookModel BookData);

    }
}
