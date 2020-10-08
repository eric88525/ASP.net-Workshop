using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookManageSystem.Model;

namespace BookManageSystem.Service
{
    public class BookService
    {
        BookManageSystem.dao.BookDao bookDao = new BookManageSystem.dao.BookDao();
        public int InsertBook(Model.Book book)
        {

            return bookDao.InsertBook(book);
        }


        public List<Model.Book> GetBookByCondtioin(Model.BookArgs arg)
        {

            return bookDao.GetBookByCondtioin(arg);

        }

        public List<string> GetBookNameList()
        {
            return bookDao.GetBookNameList();

        }

        public Boolean UpdateBook(Book book)
        {
            return bookDao.UpdateBook(book);
        }

        public Boolean CheckBookIdExist(string bookId)
        {
            return bookDao.CheckBookIdExist(bookId);
        }


        public string DeleteBookById(string bookId)
        {
            return bookDao.DeleteBookById(bookId);
        }

        public Book GetBookEditDataById(string bookId)
        {
            return bookDao.GetBookEditDataById(bookId);
        }

        public List<Book> GetBookRecordById(string bookId)
        {

            return bookDao.GetBookRecordById(bookId);
        }
    }
}
