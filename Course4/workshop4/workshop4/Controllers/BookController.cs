using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using workshop4.Models;

namespace workshop4.Controllers
{
    public class BookController : Controller
    {


        Models.CodeService codeService = new Models.CodeService();
        Models.BookService bookService = new Models.BookService();


        // GET: Book
        public ActionResult Index()
        {
            return RedirectToAction("SearchBook", "Book");
            //  return View();
        }

        [HttpGet()]
        public ActionResult InsertBook()
        {

            ViewBag.BookClassIdData = codeService.GetCodeTable("BookClass");
            return View();
        }


        [HttpPost()]
        public ActionResult InsertBook(Models.Book book)
        {
           // 時間欄位要填
            bookService.InsertBook(book);
            /*    if (ModelState.IsValid)
                {

                    TempData["message"] = "存檔成功";
                }*/
            ViewBag.BookClassIdData = codeService.GetCodeTable("BookClass");
            return View(book);
        }

        [HttpGet()]
        public ActionResult SearchBook()
        {
            ViewBag.BookClassData = codeService.GetCodeTable("BookClass");
            ViewBag.BookKeeperData = codeService.GetCodeTable("UserName");
            ViewBag.BookStatusData = codeService.GetCodeTable("BookStatus");

            return View("SearchBook");
        }

        [HttpPost()]
        public ActionResult SearchBook(Models.BookArgs bookArg)
        {

            ViewBag.BookClassData = codeService.GetCodeTable("BookClass");
            ViewBag.BookKeeperData = codeService.GetCodeTable("UserName");
            ViewBag.BookStatusData = codeService.GetCodeTable("BookStatus");
            ViewBag.SearchResult = bookService.GetBookByCondtioin(bookArg);

            return View("SearchBook");
        }


        [HttpPost()]
        public ActionResult DeleteBook(string bookId)
        {
            try
            {
                bookService.DeleteBookById(bookId);
                return this.Json(true);
            }
            catch (Exception e)
            {

                return this.Json(false);
            }

        }

        [HttpGet()]
          public ActionResult EditBook(string bookId)
          {
                // 判斷數字轉型ok?
              if (string.IsNullOrEmpty(bookId) )
              {
                  return RedirectToAction("SearchBook", "Book");
                 // return View("SearchBook");
              }

              Models.Book book = new Book();
              book = bookService.GetBookEditDataById(bookId);
              ViewBag.BookStatusData = codeService.GetCodeTable("BookStatus");
              ViewBag.BookClassIdData = codeService.GetCodeTable("BookClass");
              ViewBag.KeeperFullNameData = codeService.GetCodeTable("KeeperFullName");



              return View(book);
          }

        [HttpPost()]
        public ActionResult EditBook(Book book)
        {


            ViewBag.BookStatusData = codeService.GetCodeTable("BookStatus");
            ViewBag.BookClassIdData = codeService.GetCodeTable("BookClass");
            ViewBag.KeeperFullNameData = codeService.GetCodeTable("KeeperFullName");


            int success = bookService.UpdateBook(book);

            return View(book);
        }

        /*   [HttpPost]
           public ActionResult EditBook(string bookId)
           {

               Models.Book book = new Book();
               book.BookAuthor = "123456";

               return View(book);
           }*/
    }
}