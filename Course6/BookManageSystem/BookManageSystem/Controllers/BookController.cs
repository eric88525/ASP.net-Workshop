using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

using BookManageSystem.Model;

namespace BookMamageSystem.Controllers
{
    public class BookController : Controller
    {


        BookManageSystem.Service.CodeService codeService = new BookManageSystem.Service.CodeService();
        BookManageSystem.Service.BookService bookService = new BookManageSystem.Service.BookService();





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
        public ActionResult InsertBook(BookManageSystem.Model.Book book)
        {


            // 時間欄位要填
            if (ModelState.IsValid)
            {
                bookService.InsertBook(book);
                TempData["message"] = "新增" + book.BookName + "成功";
            }
            return View(book);
        }

        [HttpGet()]
        public ActionResult SearchBook()
        {


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

            // return result or no var
            JsonResult j = Json(codeService.GetCodeTable(type));
            return j;
        }



        [HttpPost()]
        public JsonResult GetSearchResult(BookManageSystem.Model.BookArgs bookArg)
        {


            List<Book> searchResult = bookService.GetBookByCondtioin(bookArg);

            JsonResult j = Json(searchResult);
            return j;
        }

        [HttpPost()]
        public ActionResult DeleteBook(string bookId)
        {

            string result = bookService.DeleteBookById(bookId);

            return this.Json(result);
   
        }

        public Boolean BookIdCorrect(string bookId)
        {
            int number;
            // 判斷數字轉型ok?
            if (string.IsNullOrEmpty(bookId) || !Int32.TryParse(bookId, out number))
            {
                TempData["message"] = "BookId not number";

                return false;
            }

            if (!bookService.CheckBookIdExist(bookId))
            {
                TempData["message"] = "BookId not exist";
                return false;

            }

            return true;
        }

        [HttpGet()]
        public ActionResult EditBook(string bookId, Boolean edit = false)
        {

            if (!BookIdCorrect(bookId))
            {
                return RedirectToAction("SearchBook", "Book");
            }

            BookManageSystem.Model.Book book = new Book();
            try
            {
                book = bookService.GetBookEditDataById(bookId);
            }
            catch (Exception e)
            {
                TempData["message"] = "BookId not exist";
                return RedirectToAction("SearchBook", "Book");
            }

            if (edit)
            {
                TempData["edit"] = true;
            }
            else
            {
                TempData["readonly"] = true;
            }


            return View(book);
        }
        [ValidateInput(false)]
        [HttpPost()]
        public ActionResult EditBook(Book book)
        {


            if (bookService.UpdateBook(book))
            {
                TempData["message"] = "Update success";

            }
            else
            {
                TempData["message"] = "Update false";
            }

            TempData["edit"] = true;

            return View(book);
        }


        [HttpGet()]
        public ActionResult GetBookRecord(string bookId)
        {
            if (!BookIdCorrect(bookId))
            {
                return RedirectToAction("SearchBook", "Book");
            }

            ViewBag.bookId = bookId;
            return View();

        }

        [HttpPost()]
        public JsonResult GetBookRecordById(string bookId)
        {

            return Json(bookService.GetBookRecordById(bookId));
        }

        [HttpPost]
        public JsonResult GetBookNameList()
        {

            return Json(this.bookService.GetBookNameList());

        }



    }
}