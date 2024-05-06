using Microsoft.AspNetCore.Mvc;
using OnlineBookShopping.BookDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineBookShopping.Data;
using OnlineBookShopping.Interfaces;
using OnlineBookShopping.Models;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using System.IO.MemoryMappedFiles;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using Microsoft.AspNetCore.Http;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace OnlineBookShopping.Controllers
{
    public class BookController : Controller
    {
        #region All Object And Constructor And Display All Book For Admin
        //private readonly INotyfService _notyf;
        private IHttpContextAccessor Accessor;
        BookDto dto = new BookDto();
        private readonly IWebHostEnvironment webHostEnvironment;
        private UserManager<IdentityUser> _userManager;
        //  private RoleManager<IdentityResult> _roleManager;
        //private AppliCationSignInManager _signinManager;
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            //List<BookModel> Books = _bookRepo.GetAllBook();
            dto.Author = _bookRepo.GetAllAuthor();
            dto.Publisher = _bookRepo.GetAllPublisher();
            dto.book = _bookRepo.GetAllBook();
            return View(dto);
        }
        private readonly OnlineBookContext _context;
        private readonly IBook _bookRepo;
    
        public BookController(OnlineBookContext context, IBook bookrepo, IWebHostEnvironment hostEnvironment,UserManager<IdentityUser> userManager, IHttpContextAccessor _accessor)
        {
            _context = context;
            _bookRepo = bookrepo;
            webHostEnvironment = hostEnvironment;
            _userManager = userManager;
            this.Accessor = _accessor;
           // _notyf = notyf;
            //_roleManager = roleManager;
        }

        #endregion

        #region Add Book 

        [Authorize]
        [Route("Create")]
        [HttpGet]
        public IActionResult Create()
        {
            // Unit unit = new Unit(); 

            dto.Author = _bookRepo.GetAllAuthor();
            dto.Publisher = _bookRepo.GetAllPublisher();
            //_notyf.Success("Success Notification");
            // dto.book = _bookRepo.GetAllBook();
            return View(dto);
        }
        [HttpPost]
        public  IActionResult Create(BookDto bookDto)
        {

            if (bookDto.BookProperty.BookTitle==null)
            {
                return NotFound();
            }
            if (bookDto.BookProperty!=null)
            {
                string uniqueFileName = UploadedFile(bookDto);
                if (uniqueFileName==null)
                {
                    return Json(new { Success = false });
                }
                else
                {
                    bookDto.BookProperty.BookImage = uniqueFileName;
                    bookDto.BookProperty.IsDeleted = true;
                    _bookRepo.AddBook(bookDto);
                    return Json(new { Success = true });
                }
            }
            #region Add Book Old Code
            //try
            //{
            //    // dto.book = _bookRepo.AddBook(bookDto.BookProperty);

            //    if (ModelState.IsValid)
            //    {

            //        string uniqueFileName = UploadedFile(bookDto);
            //        bookDto.BookProperty.BookImage = uniqueFileName;
            //        bookDto.BookProperty.IsDeleted = false;
            //        _bookRepo.AddBook(bookDto);
            //        return Json(new { Success = true });
            //    }
            //}
            //catch (Exception)
            //{

            //    throw;
            //}
            #endregion

            return Json(new { Success = false });
        }

        //Add Book For API And Upload Photo for api
        #region Upload Photo When Add Book
        private string UploadedFileAPI(BookModel BookData)
        {
            string uniqueFileName = null;







            if (BookData.BookImage != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + BookData.BookImage;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                System.IO.File.Copy("C:\\Users\\pdurgesh\\Downloads\\" + BookData.BookImage, filePath);

            }
            return uniqueFileName;
        }
        #endregion

        [HttpPost]
        public IActionResult AddBook(BookModel BookData)
        {

            if (BookData.AuthorId == null)
            {
                return NotFound();
            }
            if (BookData != null)
            {
                string uniqueFileName = UploadedFileAPI(BookData);

                if (uniqueFileName == null)
                {
                    return Json(new { Success = false });
                }
                else
                {
                    BookData.BookImage = uniqueFileName;
                    BookData.IsDeleted = false;
                    _bookRepo.AddBookAPI(BookData);
                    return Json(new { Success = true });
                }
            }
            #region Add Book Old Code
            //try
            //{
            //    // dto.book = _bookRepo.AddBook(bookDto.BookProperty);

            //    if (ModelState.IsValid)
            //    {

            //        string uniqueFileName = UploadedFile(bookDto);
            //        bookDto.BookProperty.BookImage = uniqueFileName;
            //        bookDto.BookProperty.IsDeleted = false;
            //        _bookRepo.AddBook(bookDto);
            //        return Json(new { Success = true });
            //    }
            //}
            //catch (Exception)
            //{

            //    throw;
            //}
            #endregion

            return Json(new { Success = false });
        }


        #endregion

        #region Upload Photo When Add Book
        private string UploadedFile(BookDto bookDto)
        {
            string uniqueFileName = null;

            if (bookDto.BookProperty.BookImage != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + bookDto.BookProperty.BookImage;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                //using (var fileStream = new FileStream(filePath, FileMode.Create))
                //{

                    //  model.ProfileImage.CopyTo(fileStream);
                    //bookDto.BookProperty.BookImage.CopyTo (  fileStream);
                    System.IO.File.Copy("C:\\Users\\pdurgesh\\Downloads\\" + bookDto.BookProperty.BookImage, filePath);
         
                //}
            }


            //string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Document");

            ////create folder if not exist
            //if (!Directory.Exists(path))
            //    Directory.CreateDirectory(path);

            ////get file extension
            //FileInfo fileInfo = new FileInfo(bookDto.BookProperty.BookImage);
            //string fileName = bookDto.BookProperty.BookImage + fileInfo.Extension;

            //string fileNameWithPath = Path.Combine(path, fileName);

            //using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
            //{
            //    bookDto.BookProperty.BookImage.CopyTo(stream);
            //}



            return uniqueFileName;
        }
        #endregion

        #region Edit Book Details
        [Authorize]
        public IActionResult EditBook(int id)
        {
            //dto.BookProperty
            // Unit unit = _unitRepo.GetUnit(id);
            BookModel BookData = _bookRepo.GetOneBook(id);
            dto.Author = _bookRepo.GetAllAuthor();
            dto.Publisher = _bookRepo.GetAllPublisher();
            dto.BookProperty = _bookRepo.GetOneBook(id);
            return View(dto);
           // return View();
        }
        [HttpPost]
        public IActionResult EditBook(BookDto bookDto)
        {
            if (bookDto.BookProperty.BookImage == null)
            {
                dto.BookProperty = _bookRepo.GetOneBook(bookDto.BookProperty.BookId);
                bookDto.BookProperty.BookImage = dto.BookProperty.BookImage;
            }
            if (ModelState.IsValid)
            {
                string uniqueFileName = UploadedFile(bookDto);
                bookDto.BookProperty.BookImage = uniqueFileName;
                bookDto.BookProperty.IsDeleted = false;
                _bookRepo.Edit(bookDto);
                // _context.Books.Add(bookDto.BookProperty);
                //   await _context.SaveChangesAsync();
                TempData["Message"] = " Book Edit Successfully";
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
            ////_bookRepo.Edit(bookDto.BookProperty);
            ////return RedirectToAction(nameof(Index));
        }

#endregion

        #region Add Author Code

        //Add Author
        [Authorize]
        [Route("AddAuthor")]
        [HttpGet]
        public IActionResult AddAuthor()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddAuthor(AuthorModel AuthorData)
        {
            if(AuthorData.AuthorName==null)
            {
                return NotFound();
            }
            if (AuthorData != null)
            {
                _bookRepo.AddAuthor(AuthorData);  
                return Json(new { Success = true });
            }
            return Json(new { Success = false });
        }

        #endregion

        #region addauthor other method

        //public async Task<IActionResult> AddAuthor(AuthorModel AuthorData)
        //{
        //    var book = await _context.Book.FindAsync(BookId);
        //    _context.Book.Remove(book);
        //    await _context.SaveChangesAsync();
        //    // return RedirectToAction(nameof(Index));
        //    return Json(new { Success = true });
        //}

        #endregion

        #region Add Publisher Code
        //Add Publisher
        [Authorize]
        [Route("AddPublisher")]
        [HttpGet]
        public IActionResult AddPublisher()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddPublisher(PublisherModel PublisherData)
        {
            if (PublisherData.PublisherName==null)
            {
                return NotFound();
            }
            if (PublisherData!=null)
            {
                _bookRepo.AddPublisher(PublisherData);
                return Json(new { Success = true });
            }
            return Json(new { Success = false });
        }

        #endregion

        #region Delete Book 
        [Authorize]
        [HttpGet]
        public IActionResult Delete(int BookId)
        {
            if (BookId<1)
            {
                return NotFound();
            }
            if (BookId>=1)
            {
                _bookRepo.Delete(BookId);
                return Json(new { Success = true });
            }
            return Json(new { Success = false });
        }
        #endregion


        #region All Book Display For Client It may be Student or customer
        //Client Side
        [Authorize]
        public IActionResult BookDisplay()
        {
            dto.Author = _bookRepo.GetAllAuthor();
            dto.Publisher = _bookRepo.GetAllPublisher();
            dto.book = _bookRepo.GetAllBook();
            return View(dto);
        }
        #endregion

        #region Order Book For Customer And Student

        //order book
        [Authorize]
        [Route("OrderBook")]
        [HttpGet]
        public IActionResult OrderBook(int id)
        { 
            BookModel BookData = _bookRepo.GetOneBook(id);
            return View(BookData);
        }

        //Order Book Method For User
        [HttpPost]
        public IActionResult OrderBook(BookModel BookData)
        {
            if (BookData.BookId<1)
            {
                return NotFound();
            }
            if (BookData!=null)
            {
                var UserName = _userManager.GetUserName(User).ToString();
                // var qty = BookData.Quantity;
                BookSellModel bookSellModel = new BookSellModel();
                bookSellModel.BookId = BookData.BookId;
                bookSellModel.UserName = UserName;
                bookSellModel.Payment = "COD";
                bookSellModel.PurchaseDate = DateTime.Now;
                bookSellModel.Quantity = (int)BookData.Quantity;
                if (BookData.BookStock >= bookSellModel.Quantity)
                {
                    _bookRepo.OrderBook(bookSellModel);
                    _bookRepo.UpdateStock(BookData);
                    return Json(new { Success = true });
                }
                return Json(new { Success=false});
            }
            return Json(new { Success = false });
        }

        #endregion

        #region manage Book Order For Admin
        //Manage Order Book For Admin
        [Authorize]
        public IActionResult ManageTransaction()
        {
            // List<BookSellModel> bookSells=  _bookRepo.GetAllOrder();
            dto.BookSell = _bookRepo.GetAllOrder();
            dto.book = _bookRepo.GetAllBook();
            return View(dto);
        }

        #endregion

        #region Book Order History For user
        //Order History For User
        [Authorize]
        public IActionResult OrderListForUser()
        {
            var UserName = _userManager.GetUserName(User).ToString();
            // List<BookSellModel> bookSells=  _bookRepo.GetAllOrder();
            dto.BookSell = _bookRepo.OrderListForUser(UserName);
            dto.book = _bookRepo.GetAllBook();
            return View(dto);
        }

        #endregion

        //Book Rental Logic and Detail Start Here

        #region Document Upload For Renting Book
        [Authorize]
       
        [HttpGet]
        public IActionResult DocumentUpload()
        {
           
            return View();
        }

        [HttpPost]
        public IActionResult DocumentUpload(DocumentsUpload Data)
        {
            string uniqueFileName = null;
            var DocumentUser=_context.UploadData.Where(x => x.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier)).FirstOrDefault();
            string testvalue = null;
            if (Convert.ToBoolean(DocumentUser))
            {
                testvalue = DocumentUser.UserId;
            }
            if (Data.FileName==null)
            {
                return NotFound();
            }
            if (Data!=null)
            {
                if (testvalue != User.FindFirstValue(ClaimTypes.NameIdentifier))
                {
                    string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Document");

                    //create folder if not exist
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    //get file extension
                    FileInfo fileInfo = new FileInfo(Data.FileName);
                    // string fileName = Data.FileName + fileInfo.Extension;
                
                        uniqueFileName = Guid.NewGuid().ToString() + "_" + Data.File.FileName + fileInfo.Extension;

                        string fileNameWithPath = Path.Combine(path, uniqueFileName);

                        using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                        {
                            Data.File.CopyTo(stream);
                        }
                        Data.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                        Data.FileName = uniqueFileName;
                        _context.UploadData.Add(Data);
                        _context.SaveChanges();
                    ViewData["Message"] = "Document Uploaded Successfully";
                    //  return Json(new { Success = true });
                    return View();
                
                }
            }
            else
            {
                return View();
                // return Json(new { Success = false });
            }
            return View();
            //return Json(new { Success = false });
        }
        #endregion

        #region Book request For User...

        [Authorize]
        [Route("BookRequest")]
        [HttpGet]
        public IActionResult BookRequest(int id)
        {
            BookModel BookData = _bookRepo.GetOneBook(id);
            return View(BookData);
        }
        [HttpPost]
        public IActionResult BookRequest(BookModel Data)
        {
            var DocumentUser = _context.UploadData.Where(x => x.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier)).FirstOrDefault();
            var count = _context.BookRent.Where(x => x.UserName == User.FindFirstValue(ClaimTypes.NameIdentifier) && x.IsReturn == false).Count();
            if (Data.BookId<1)
            {
                return NotFound();
            }

            if (Data!=null)
            {
                BookRentalModel bookRentalModel = new()
                {
                    BookId = Data.BookId,
                    UserName = _userManager.GetUserName(User).ToString(),
                    BookRequestDate = DateTime.Now,
                    ReturnDate = null,
                    IssueDate = null,
                    Fine = 0,
                    IsIssued = false,
                    IsReturn = false
                };
                if (Convert.ToBoolean(DocumentUser.DocumentId))
                {
                    if (Data.BookStock > 0)
                    {
                        if (count <= 3)
                        {
                            _bookRepo.BookRent(bookRentalModel);
                            return Json(new { Success = true });
                        }
                    }
                    return Json(new { Success = false });
                }
                return Json(new { Success = false });
            }
            return Json(new { Success = false });
        }

        #endregion

        #region Display BookRequest List For User
        [Authorize]
        public IActionResult BookRequestListForUser()
        {
            //var UserName = _userManager.GetUserName(User).ToString();
            var UserName = _userManager.GetUserName(User).ToString();
            // List<BookSellModel> bookSells=  _bookRepo.GetAllOrder();
            dto.BookRentals = _bookRepo.GetAllRentalBookByUser(UserName);
           // dto.BookSell = _bookRepo.OrderListForUser(UserName);
            dto.book = _bookRepo.GetAllBook();
            return View(dto);
        }
        #endregion
        //Manage Book Rent For Admin

        #region Issue And return Book Admin Side
        [Authorize]
        public IActionResult ManageBookRequest()
        {
            dto.BookRentals = _bookRepo.GetAllRentalBook();
            dto.book = _bookRepo.GetAllBook();
            return View(dto);
        }
        [HttpGet]
        public IActionResult ManageBookRequest1(int BookRentId)
        {
            if (BookRentId<1)
            {
                return NotFound();
            }
            if (BookRentId>=1)
            {
                var count = _context.BookRent.Where(x => x.UserName == User.FindFirstValue(ClaimTypes.NameIdentifier) && x.IsReturn == false).Count();

                var bookid = _context.BookRent.Where(u => u.BookRentId == BookRentId).Select(u => u.BookId).SingleOrDefault();
                BookRentalModel bookRentalModel = new BookRentalModel();
                bookRentalModel.IssueDate = DateTime.Now;
                bookRentalModel.ReturnDate = DateTime.Now.AddDays(5);
                bookRentalModel.BookRentId = BookRentId;
                // var test = bookRentalModel.ReturnDate - DateTime.Now;
                bookRentalModel.DelayDay = 0;
                bookRentalModel.BookRentId = BookRentId;
                bookRentalModel.IsIssued = true;
                bookRentalModel.IsReturn = false;
                BookModel bookModel = new BookModel();
                bookModel.BookId = (int)bookid;
                if (count <= 3)
                {
                    _bookRepo.BookIssue(bookRentalModel);
                    _bookRepo.UpDateBookStock(bookModel);
                    return Json(new { Success = true });
                }
                else
                {
                    return Json(new { Success = false });
                }
            }
            return Json(new { Success = false });
        }
      
        public IActionResult ManageBookReturn(int BookRentId)
        {
            int fine = 1;
            if (BookRentId < 1)
            {
                return NotFound();
            }
            if (BookRentId>=1)
            {
                var bookid = _context.BookRent.Where(u => u.BookRentId == BookRentId).Select(u => u.BookId).SingleOrDefault();
                DateTime retudate = (DateTime)_context.BookRent.Where(u => u.BookRentId == BookRentId).Select(u => u.ReturnDate).SingleOrDefault();
                // var retudate = _context.BookRent.Where(u => u.BookRentId == id).Select(u => u.ReturnDate).SingleOrDefault();
                BookRentalModel bookRentalModel = new BookRentalModel();
                BookModel bookModel = new BookModel();
                bookModel.BookId = (int)bookid;
                bookRentalModel.IsIssued = false;
                bookRentalModel.IsReturn = true;
                bookRentalModel.BookRentId = BookRentId;
                int returnmonth = retudate.Month;
                int returnday = retudate.Day;
                int currentmonth = DateTime.Now.Month;
                int currentday = DateTime.Now.Day;
                bookRentalModel.DelayDay = 0;
                if (DateTime.Now > retudate)
                {

                    fine = 5 * (returnday - currentday);
                    if (fine != 1)
                    {
                        if (returnday >= currentday && returnmonth == currentmonth)
                        {
                            bookRentalModel.Fine = 0;
                        }
                        if (currentday > returnday && returnmonth == currentmonth)
                        {
                            bookRentalModel.Fine = fine;
                            bookRentalModel.DelayDay = returnday - currentday;
                        }
                    }
                    else
                    {
                        bookRentalModel.Fine = 0;
                    }

                    _bookRepo.BookReturn(bookRentalModel);
                    _bookRepo.ReturnUpDateBookStock(bookModel);
                    return Json(new { Success = true });
                }
                else
                {
                    _bookRepo.BookReturn(bookRentalModel);
                    _bookRepo.ReturnUpDateBookStock(bookModel);
                    return Json(new { Success = true });
                }
            }
                return Json(new { Success = false });
        }


        #endregion


        #region Role Permission For Admin
        [Authorize]
        public IActionResult RolePermission()
        {
            ViewBag.role = _userManager.Users.ToList();
            return View();
        }
        [HttpPost]
        public IActionResult RolePermission(AssignRole assignRole)
        {
            // object p = await _userManager.AddToRoleAsync(assignRole.UserId, assignRole.RoleId);

            return View();
        }
        #endregion
        public IActionResult TokenInvalid()
        {
            ViewData["Message"] = "Inavalid Token";
            return View();
        }
    }
}
