using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Management;
using System.Web.Mvc;

namespace workshop4.Models
{
	public class BookService
	{

		private string GetDBConnectionString()
		{
			return
				System.Configuration.ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString.ToString();
		}



		public int InsertBook(Models.Book book)
		{

			string sql = @" INSERT INTO BOOK_DATA(BOOK_NAME, BOOK_AUTHOR, BOOK_PUBLISHER, BOOK_NOTE, BOOK_BOUGHT_DATE, BOOK_CLASS_ID, BOOK_STATUS )
			VALUES( @BookName, @BookAuthor , @BookPublisher, @BookNote , @BookBoughtDate, @BookClassId, @BookStatus)";

			int BookId;
			using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
			{
				conn.Open();
				SqlCommand cmd = new SqlCommand(sql, conn);

				cmd.Parameters.Add(new SqlParameter("@BookName", book.BookName));
				cmd.Parameters.Add(new SqlParameter("@BookAuthor", book.BookAuthor == null ? string.Empty : book.BookAuthor));
				cmd.Parameters.Add(new SqlParameter("@BookPublisher", book.BookPublisher == null ? string.Empty : book.BookPublisher));
				cmd.Parameters.Add(new SqlParameter("@BookNote", book.BookNote == null ? string.Empty : book.BookNote));
				cmd.Parameters.Add(new SqlParameter("@BookBoughtDate", book.BookBoughtDate));
				cmd.Parameters.Add(new SqlParameter("@BookClassId", book.BookClassId));
				cmd.Parameters.Add(new SqlParameter("@BookStatus", 'A'));

				BookId = Convert.ToInt32(cmd.ExecuteNonQuery());
				conn.Close();
			}
			return BookId;
		}
		/// <summary>
		/// 依照條件取得book資料
		/// </summary>
		/// <returns></returns>
		public List<Models.Book> GetBookByCondtioin(Models.BookArgs arg)
		{
			DataTable dt = new DataTable();
			string sql = @"SELECT                           
	                            DISTINCT bd.BOOK_ID AS BookId
                               ,bc.BOOK_CLASS_NAME AS BookClass
                               ,bd.BOOK_NAME AS BookName
                               ,FORMAT(bd.BOOK_BOUGHT_DATE ,'yyyy-MM-dd') AS BookBoughtDate
                               ,bc1.CODE_NAME AS BookStatus
                               ,mm.USER_ENAME AS UserEName

                            FROM BOOK_DATA bd
                            INNER JOIN BOOK_CLASS bc
	                            ON bd.BOOK_CLASS_ID = bc.BOOK_CLASS_ID
                            LEFT JOIN BOOK_CODE bc1
	                            ON bd.BOOK_STATUS = bc1.CODE_ID
		                            AND bc1.CODE_TYPE_DESC = '書籍狀態'
                            LEFT JOIN MEMBER_M mm
	                            ON bd.BOOK_KEEPER = mm.USER_ID

                            LEFT JOIN BOOK_LEND_RECORD blr
	                            ON bd.BOOK_ID = blr.BOOK_ID
                           
						WHERE (bc.BOOK_CLASS_ID = @BookClassId OR @BookClassId='')
						AND BOOK_NAME LIKE ('%' + @BookName + '%')
						AND (USER_ENAME = @BookKeeper  OR  @BookKeeper='')
						AND (CODE_ID = @BookStatus OR  @BookStatus='')";

			using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
			{
				conn.Open();
				SqlCommand cmd = new SqlCommand(sql, conn);
				cmd.Parameters.Add(new SqlParameter("@BookClassId", arg.BookClassId == null ? string.Empty : arg.BookClassId));
				cmd.Parameters.Add(new SqlParameter("@BookName", arg.BookName == null ? string.Empty : arg.BookName));
				cmd.Parameters.Add(new SqlParameter("@BookKeeper", arg.BookKeeper == null ? string.Empty : arg.BookKeeper));
				cmd.Parameters.Add(new SqlParameter("@BookStatus", arg.BookStatus == null ? string.Empty : arg.BookStatus));


				SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
				sqlAdapter.Fill(dt);
				conn.Close();
			}
			return this.MapBookDataToList(dt);

		}

        //boolean
        /// <summary>
        /// return true:success
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        /// return bool
        public int UpdateBook(Book book)
        {
            int success = 0;


            string sql = @"UPDATE BOOK_DATA
                            SET BOOK_NAME = @BookName
                               ,BOOK_AUTHOR = @BookAuthor
                               ,BOOK_PUBLISHER = @BookPublisher
                               ,BOOK_NOTE = @BookNote
                               ,BOOK_BOUGHT_DATE = @BookBoughtDate
                               ,BOOK_CLASS_ID = @BookClassId
                               ,BOOK_STATUS = @BookStatus
                               ,BOOK_KEEPER = @BookKeeperId
                            WHERE BOOK_ID = @BookId;
";

            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@BookName", book.BookName == null ? string.Empty : book.BookName));
                cmd.Parameters.Add(new SqlParameter("@BookAuthor", book.BookAuthor == null ? string.Empty : book.BookAuthor));
                cmd.Parameters.Add(new SqlParameter("@BookPublisher", book.BookPublisher == null ? string.Empty : book.BookPublisher));
                cmd.Parameters.Add(new SqlParameter("@BookNote", book.BookNote == null ? string.Empty : book.BookNote));
                cmd.Parameters.Add(new SqlParameter("@BookBoughtDate", book.BookBoughtDate == null ? string.Empty : book.BookBoughtDate));
                cmd.Parameters.Add(new SqlParameter("@BookClassId", book.BookClassId == null ? string.Empty : book.BookClassId));
                cmd.Parameters.Add(new SqlParameter("@BookStatus", book.BookStatus == null ? string.Empty : book.BookStatus));
                cmd.Parameters.Add(new SqlParameter("@BookKeeperId", book.BookKeeperId == null ? string.Empty : book.BookKeeperId));
                cmd.Parameters.Add(new SqlParameter("@BookId", book.BookId));

                success = Convert.ToInt32(cmd.ExecuteScalar());
                conn.Close();
            }


            return success;

        }


		/// <summary>
		/// 刪除客戶
		/// </summary>
		[HttpPost()]
		public void DeleteBookById(string bookId)
		{
			try
            {


                string sql = @"DELETE FROM BOOK_DATA WHERE BOOK_ID = @BookId";
                using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.Add(new SqlParameter("@BookId", bookId));
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }

            }

			catch (Exception ex)
            {
                throw ex;
            }
		}

		/// <summary>
		/// Map資料進List
		/// </summary>
		/// <param name="employeeData"></param>
		/// <returns></returns>

		private List<Models.Book> MapBookDataToList(DataTable bookData)
		{
			List<Models.Book> result = new List<Book>();
			foreach (DataRow row in bookData.Rows)
			{
				result.Add(new Book()
				{

                    BookId =   row["BookId"].ToString(),
					BookClass = row["BookClass"].ToString(),
					BookName = row["BookName"].ToString(),
					BookBoughtDate = row["BookBoughtDate"].ToString(),
					BookStatus = row["BookStatus"].ToString(),
                    UserEName = row["UserEName"].ToString()


				});
			}

			return result;

		}

        private Book MapBookDataToEdit(DataTable bookData)
        {
            Book book = new Book();
            string test = bookData.Rows[0]["BookId"].ToString();
            foreach (DataRow row in bookData.Rows)
            {
                book.BookId = row["BookId"].ToString();
                book.BookName = row["BookName"].ToString();
                book.BookAuthor = row["BookAuthor"].ToString();
                book.BookPublisher = row["BookPublisher"].ToString();
                book.BookNote = row["BookNote"].ToString();
                book.BookBoughtDate = row["BookBoughtDate"].ToString();
                book.BookClassId = row["BookClassId"].ToString();
                book.BookStatus = row["BookStatus"].ToString();
                book.BookKeeperId = row["KeeperId"].ToString();

            }

            return book;

        }
        public Book GetBookEditDataById(string bookId)
        {

            DataTable dt = new DataTable();
            string sql = @"SELECT 
                            bd.BOOK_ID AS BookId,
                            bd.BOOK_NAME AS BookName,
                            bd.BOOK_AUTHOR AS BookAuthor,
                            bd.BOOK_PUBLISHER AS BookPublisher,
                            bd.BOOK_NOTE AS BookNote,
                            FORMAT(bd.BOOK_BOUGHT_DATE ,'yyyy-MM-dd') AS BookBoughtDate,
                            bd.BOOK_CLASS_ID AS BookClassId,
                            bd.BOOK_STATUS AS BookStatus,
                            bd.BOOK_KEEPER AS KeeperId
                            FROM BOOK_DATA bd
                            WHERE bd.BOOK_ID=@BookId ";//
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@BookId", bookId));
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }
            

            return this.MapBookDataToEdit(dt);


        }


	}
}