using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using workshop4.Models;

namespace workshop4.Controllers
{
    public class BookController : Controller
    {


        Models.CodeService codeService = new Models.CodeService();
        Models.BookService bookService = new Models.BookService();



        /// <summary>
        /// TEST
        /// </summary>
        /// <returns></returns>
        public ActionResult Test()
        {
            Models.BookArgs arg = new BookArgs();
            return View(arg);
            //  return View();
        }



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

        [ValidateInput(false)]
        [HttpPost()]
        public ActionResult InsertBook(Models.Book book)
        {
           

            // 時間欄位要填
            if (ModelState.IsValid)
            {
                bookService.InsertBook(book);
                TempData["message"] = "新增" + book.BookName +  "成功";
            }
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


        /// <summary>
        /// Get select list by type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpPost()]
        public JsonResult GetSelectList(string type)
        {
            JsonResult j = Json(codeService.GetCodeTable(type));
            return j;
        }



        [HttpPost()]
        public JsonResult GetSearchResult(Models.BookArgs bookArg)
        {


            List<Book> searchResult = bookService.GetBookByCondtioin(bookArg);

            JsonResult j = Json(searchResult);
            return j;
        }

        [HttpPost()]
        public ActionResult DeleteBook(string bookId)
        {
            if (bookService.DeleteBookById(bookId))
            {
                
                return this.Json(true);
            }
            else
            {
                return new EmptyResult();
            }


        }

        [HttpGet()]
        public ActionResult EditBook(string bookId)
        {

            int number;
            // 判斷數字轉型ok?
            if (string.IsNullOrEmpty(bookId) || !Int32.TryParse(bookId, out number))
            {
                TempData["message"] = "BookId not number";
                return RedirectToAction("SearchBook", "Book");
             
            }

            if (!bookService.CheckBookIdExist(bookId))
            {
                TempData["message"] = "BookId not exist";
                return RedirectToAction("SearchBook", "Book");
            }



            Models.Book book = new Book();
            try
            {
                book = bookService.GetBookEditDataById(bookId);
            }
            catch (Exception e)
            {
                TempData["message"] = "BookId not exist";
                return RedirectToAction("SearchBook", "Book");
            }


            ViewBag.BookStatusData = codeService.GetCodeTable("BookStatus");
            ViewBag.BookClassIdData = codeService.GetCodeTable("BookClass");
            ViewBag.KeeperFullNameData = codeService.GetCodeTable("KeeperFullName");



            return View(book);
        }
        [ValidateInput(false)]
        [HttpPost()]
        public ActionResult EditBook(Book book)
        {

            

            ViewBag.BookStatusData = codeService.GetCodeTable("BookStatus");
            ViewBag.BookClassIdData = codeService.GetCodeTable("BookClass");
            ViewBag.KeeperFullNameData = codeService.GetCodeTable("KeeperFullName");


            if (bookService.UpdateBook(book))
            {
                TempData["message"] = "Update success";
            }
            else
            {
                TempData["message"] = "Update false";
            }


            return View(book);
        }


        [HttpGet()]
        public ActionResult CheckBookRecord(string bookId)
        {
            int number;
            // 判斷數字轉型ok?
            if (string.IsNullOrEmpty(bookId) || !Int32.TryParse(bookId, out number))
            {
                TempData["message"] = "BookId not correct";
                return RedirectToAction("SearchBook", "Book");
 
            }

            if (!bookService.CheckBookIdExist(bookId))
            {
                TempData["message"] = "BookId not exist";
                return RedirectToAction("SearchBook", "Book");
            }

            try
            {
                ViewBag.RecordResult = bookService.GetBookRecordById(bookId);
            }
            catch (Exception e)
            {
                TempData["message"] = "BookId not exist";
                return RedirectToAction("SearchBook", "Book");
            }
            return View();

        }

    }
}