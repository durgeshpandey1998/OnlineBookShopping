using Microsoft.AspNetCore.Mvc;
using OnlineBookShopping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineBookShopping.Controllers
{
    public class CustomerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(AuthorModel AuthorData)
        {
            return View();
        }
        public IActionResult GetAllBook()
        {
            return View();
        }
    }
}
