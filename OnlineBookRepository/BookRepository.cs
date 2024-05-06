using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineBookShopping.BookDTO;
using OnlineBookShopping.Data;
using OnlineBookShopping.Interfaces;
using OnlineBookShopping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineBookShopping.OnlineBookRepository
{
    public class BookRepository: IBook
    {
        BookDto dto = new BookDto();
        private UserManager<IdentityUser> _userManager;
        private readonly OnlineBookContext _context;
        public BookRepository(OnlineBookContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        #region Admin Pannel  Add Book , Manage Order History

        public BookDto AddBook(BookDto bookDto)
        {
            _context.Books.Add(bookDto.BookProperty);
            _context.SaveChanges();
            return bookDto;
        }

        public List<AuthorModel> GetAllAuthor()
        {
            List<AuthorModel> authors = _context.AuthorModel.ToList();
            return authors;
        }

        public List<BookModel> GetAllBook()
        {
            List<BookModel> books = _context.Books.Where(x=>x.IsDeleted==true).ToList();

           // List<BookModel> books1 = _context.Books.Where(u => u.IsDeleted == false).Select(u => u.AuthorId).ToList();
            //            List<BookModel> books = _context.Books
            //.Where(x => x.AuthorId == x.Author.AuhtorId && x.PublisherId == x.Publisher.PublisherId)
            //.Select(x => x.Author.AuthorName).ToList();


            return books;
        }

        public List<PublisherModel> GetAllPublisher()
        {
            List<PublisherModel> publisher = _context.PublisherModel.ToList();
            return publisher;
        }

        public BookModel GetBook(int id)
        {
            BookModel books = _context.Books.Where(u => u.BookId == id).FirstOrDefault();
            return books;
        }

        public BookDto Edit(BookDto bookDto)
        {
            //_context.Books.Attach(bookDto.BookProperty);
            //_context.Entry(bookDto.BookProperty).State = EntityState.Modified;
            _context.Books.Attach(bookDto.BookProperty);
            _context.Entry(bookDto.BookProperty).State = EntityState.Modified;
            _context.SaveChanges();
            return bookDto;
        }

        public BookModel EditAPI(BookModel BookData)
        {
            BookModel obj = new BookModel();

            obj = _context.Books.FirstOrDefault(x=> x.BookId == BookData.BookId);

           // obj.BookId = BookData.BookId;
            obj.IsDeleted = BookData.IsDeleted;
            obj.Quantity = BookData.Quantity;
            obj.PublisherId = BookData.PublisherId;
            obj.AuthorId = BookData.AuthorId;
            obj.TotalPage = BookData.TotalPage;
            obj.BookStock = BookData.BookStock;
            obj.Description = BookData.Description;
            obj.BookImage = BookData.BookImage;
            obj.BookCategory = BookData.BookCategory;
            obj.BookTitle = BookData.BookTitle;

            _context.Books.Update(obj);



           /* string query = "UPDATE [dbo].[Books]" +
                "SET[BookTitle] = '" +BookData.BookTitle+
                "',[BookCategory] = '" + BookData.BookCategory +
                "' ,[BookImage] = '" + BookData.BookImage +
                "' ,[Description] = '" + BookData.Description +
                " ',[BookStock] =" + BookData.BookStock +
                ",[TotalPage] =  " + BookData.TotalPage +
                " ,[AuthorId] = " + BookData.AuthorId +
                " ,[PublisherId] =  " + BookData.PublisherId +
                " ,[Quantity] =  " + BookData.Quantity +
                " ,[IsDeleted] =  '" + BookData.IsDeleted +
                "'WHERE BookId = "+BookData.BookId;
           
            var edit = _context.Books.FromSqlRaw(query);*/
            _context.SaveChanges();
            return BookData;
        }

        public BookModel GetOneBook(int id)
        {
            // Unit unit = _context.Units.Where(u => u.Id == id).FirstOrDefault();
             BookModel BookData = _context.Books.Where(u => u.BookId == id).FirstOrDefault();
            #region innerjoin in linque query
            //        var query = _context.Books
            //.Join(
            //    _context.AuthorModel,
            //    book => book.BookId,
            //    author => author.AuhtorId,
            //    (book, author) => new
            //    {
            //        AuthorName = author.AuthorName,
            //        BookTitle=book.BookTitle,

            //    }
            //).ToList();

            //var innerJoin = _context.Books.AsQueryable();
            //innerJoin =((IQueryable<BookModel>)(from b in _context.Books
            //               join a in _context.AuthorModel on b.AuthorId equals a.AuhtorId
            //               select new
            //               {
            //                   BookId = Convert.ToInt32(b.BookId),
            //                   BookTitle = b.BookTitle.ToString(),
            //                   AuthorName = a.AuthorName.ToString()
            //               }));



            //var Track = (from t in _context.Books
            //             join il in _context.AuthorModel
            //             on t.AuthorId equals il.AuhtorId
            //             join i in _context.PublisherModel
            //             on t.PublisherId equals i.PublisherId
            //             where t.BookId == id
            //             select (new
            //             {
            //                 BookTitle = t.BookTitle,
            //                 AuthorName = il.AuthorName,
            //                 PublisherName = i.PublisherName
            //             })
            //            ).ToList().FirstOrDefault();
            #endregion

            return BookData;
        }

        public BookModel Delete(int BookId)
        {
            #region extra work
            //var bookId = _context.Books.Where(x=>x.BookI);
            //_context.Books.Attach(BookData);
            //_context.Entry(BookData).State = EntityState.Deleted;
            //_context.SaveChanges();
            //return BookData;
            //BookModel bookModel = _context.Set<BookModel>().SingleOrDefault(c => c.BookId == BookData.BookId);
            //_context.Entry(bookModel).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            //_context.SaveChanges();
            //return bookModel;
            //int id=7;
            //var bookModel = _context.Books.Where(a => a.BookId == id).FirstOrDefault();
            //_context.Books.Remove(bookModel);
            //_context.SaveChanges();
            //return bookModel;
            #endregion
            try
            {
                BookModel obj = new BookModel();
                obj = _context.Books.FirstOrDefault(x => x.BookId == BookId);
                obj.IsDeleted = false;
                _context.Books.Update(obj);
                _context.SaveChanges();

                return obj;
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        public AuthorModel AddAuthor(AuthorModel AuthorData)
        {
            _context.AuthorModel.Add(AuthorData);
            _context.SaveChanges();
            return AuthorData;
        }

        public PublisherModel AddPublisher(PublisherModel PublisherData)
        {
            _context.PublisherModel.Add(PublisherData);
            _context.SaveChanges();
            return PublisherData;
        }

        public BookSellModel OrderBook()
        {
            throw new NotImplementedException();
        }

        public BookSellModel OrderBook(BookSellModel BookSellData)
        {
            _context.BookSell.Add(BookSellData);
            _context.SaveChanges();
            return BookSellData;
        }


        //Update stock when purchase book
        public BookModel UpdateStock(BookModel BookData)
        {
            #region Update stock query  
            //var user = new User() { Id = userId, Password = password };
            //using (var db = new MyEfContextName())
            //{
            //    db.Users.Attach(user);
            //    db.Entry(user).Property(x => x.Password).IsModified = true;
            //    db.SaveChanges();
            //}
            //var record6 = _context.Books.Where(book => book.BookId== BookData.BookId).FirstOrDefault();


            //_context.Books.Attach(BookData);
            //_context.Entry(BookData).Property(x=>(x.BookStock)-(x.Quantity)).IsModified=true;
            //_context.SaveChanges();
            #endregion

            var querydata = new BookModel { BookId = BookData.BookId, BookStock =( BookData.BookStock-(int)BookData.Quantity)};
            _context.Books.Attach(querydata);
         _context.Entry(querydata).Property(r => r.BookStock).IsModified = true;
            _context.SaveChanges();

            return BookData;
        }

        public List<BookSellModel> GetAllOrder()
        {
            List<BookSellModel> Booksell = _context.BookSell.ToList();
            return Booksell;
        }

        public List<BookSellModel> OrderListForUser(string username)
        {
            List<BookSellModel> Booksell = _context.BookSell.Where(x => x.UserName == username).ToList();
            // _context.Books.Where(x => x.IsDeleted == false).ToList();
            return Booksell;
        }

        #endregion

        #region Rental Book Logic And Return Book
        //Book Rent Request And Return 
        public BookRentalModel BookRent(BookRentalModel BookRequest)
        {
            _context.BookRent.Add(BookRequest);
            _context.SaveChanges();
            return BookRequest;
        }

        public List<BookRentalModel> GetAllRentalBook()
        {
            List<BookRentalModel> BookRequest = _context.BookRent.ToList();
            return BookRequest;
        }

        public List<BookRentalModel> GetAllRentalBookByUser(string username)
        {
            List<BookRentalModel> bookRentalModels = _context.BookRent.Where(x => x.UserName == username).ToList();
            return bookRentalModels;
        }

        public BookRentalModel BookIssue(BookRentalModel BookIssue)
        {

           // var user = db.users.Find(userId);
            var user = _context.BookRent.Find(BookIssue.BookRentId);
            user.IsIssued = true;
            user.IsReturn = false;
            user.IssueDate = BookIssue.IssueDate;
            user.ReturnDate = BookIssue.ReturnDate;
            _context.Entry(user).State = EntityState.Modified;
         
            //var querydata = new BookRentalModel { BookRentId = BookIssue.BookRentId, IsIssued = true ,IsReturn=false,IssueDate=BookIssue.IssueDate,ReturnDate=BookIssue.ReturnDate};
            //_context.BookRent.Attach(querydata);
            //_context.Entry(querydata).Property(r => r.IsIssued).IsModified = true;

            _context.SaveChanges();
            return BookIssue;
        }

        public BookRentalModel BookReturn(BookRentalModel BookReturn)
        {
            // var querydata = new BookRentalModel { BookRentId = BookReturn.BookRentId, IsIssued = false, IsReturn = true };
            var querydata = _context.BookRent.Find(BookReturn.BookRentId);
            querydata.IsIssued = BookReturn.IsIssued;
            querydata.IsReturn = BookReturn.IsReturn;
            querydata.DelayDay = BookReturn.DelayDay;
            querydata.Fine = BookReturn.Fine;
            querydata.ReturnDate = BookReturn.ReturnDate;
            _context.Entry(querydata).State = EntityState.Modified;
            //_context.BookRent.Attach(querydata);
            //_context.Entry(querydata).Property(r => r.IsIssued).IsModified = true;
            _context.SaveChanges();
            return BookReturn;
        }

        public BookModel UpDateBookStock(BookModel BookData)
        {
            BookModel querydata = new BookModel();
            querydata= _context.Books.FirstOrDefault(x=>x.BookId==BookData.BookId);
            querydata.BookStock = querydata.BookStock - 1;
            _context.Books.Update(querydata);
           
            _context.SaveChanges();
            return BookData;
        }
        public BookModel ReturnUpDateBookStock(BookModel BookData)
        {
            BookModel querydata = new BookModel();
         querydata = _context.Books.FirstOrDefault(x => x.BookId == BookData.BookId);
            querydata.BookStock = querydata.BookStock + 1;
            _context.Books.Update(querydata);
            _context.SaveChanges();
            return BookData;
        }

        public BookModel AddBookAPI(BookModel BookData)
        {
            _context.Books.Add(BookData);
            _context.SaveChanges();
            return BookData;
        }

        #endregion
    }
}
