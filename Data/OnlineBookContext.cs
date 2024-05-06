using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineBookShopping.Models;
using OnlineBookShopping.BookDTO;

namespace OnlineBookShopping.Data
{
    public class OnlineBookContext: IdentityDbContext
    {
        public OnlineBookContext(DbContextOptions options) : base(options)
        {

        }
        public virtual DbSet<BookModel> Books { get; set; }
        public virtual DbSet<OnlineBookShopping.Models.AuthorModel> AuthorModel { get; set; }
        public virtual DbSet<OnlineBookShopping.Models.PublisherModel> PublisherModel { get; set; }
        public virtual DbSet<BookRentalModel> BookRent { get; set; }
        public virtual DbSet<BookSellModel> BookSell { get; set; }
        public virtual DbSet<DocumentsUpload> UploadData { get; set; }
        public DbSet<OnlineBookShopping.Models.AssignRole> AssignRole { get; set; }
        public DbSet<OnlineBookShopping.Models.PassRecovery> PassRecovery { get; set; }
        
        
    }
}
