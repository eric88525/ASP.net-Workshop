using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookManageSystem.Model;

namespace BookManageSystem.dao
{
	public  class BookDao
	{


		private string GetDBConnectionString()
		{

			return BookManageSystem.Common.ConfigTool.GetDBConnectionString("DBConn");
		}



		public int InsertBook(Model.Book book)
		{

			string sql = @" INSERT INTO BOOK_DATA(BOOK_NAME, BOOK_AUTHOR, BOOK_PUBLISHER, 
								BOOK_NOTE, BOOK_BOUGHT_DATE, BOOK_CLASS_ID, BOOK_STATUS,   
								CREATE_DATE, CREATE_USER, MODIFY_DATE, MODIFY_USER )
							VALUES( @BookName, @BookAuthor , @BookPublisher, @BookNote ,
								@BookBoughtDate, @BookClassId, @BookStatus,
								@CreateDate , @CreateUser , @ModifyDate , @ModifyUser)";

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
				// about time
				cmd.Parameters.Add(new SqlParameter("@CreateDate", DateTime.Now));
				cmd.Parameters.Add(new SqlParameter("@CreateUser", "admin"));
				cmd.Parameters.Add(new SqlParameter("@ModifyDate", DateTime.Now));
				cmd.Parameters.Add(new SqlParameter("@ModifyUser", "admin"));

				BookId = Convert.ToInt32(cmd.ExecuteNonQuery());
				conn.Close();
			}
			return BookId;
		}
		/// <summary>
		/// 依照條件取得book資料
		/// </summary>
		/// <returns></returns>
		public List<Model.Book> GetBookByCondtioin(Model.BookArgs arg)
		{
			DataTable dt = new DataTable();
			string sql = @"SELECT
							bd.BOOK_ID AS BookId
						   ,bc.BOOK_CLASS_NAME AS BookClass
						   ,bd.BOOK_NAME AS BookName
						   ,FORMAT(bd.BOOK_BOUGHT_DATE, 'yyyy-MM-dd') AS BookBoughtDate
						   ,bc1.CODE_NAME AS BookStatus
						   ,mm.USER_ENAME AS UserEName

						FROM BOOK_DATA bd
						INNER JOIN BOOK_CLASS bc
							ON bd.BOOK_CLASS_ID = bc.BOOK_CLASS_ID
						INNER JOIN BOOK_CODE bc1
							ON bd.BOOK_STATUS = bc1.CODE_ID
								AND bc1.CODE_TYPE = 'BOOK_STATUS'
						LEFT JOIN MEMBER_M mm
							ON bd.BOOK_KEEPER = mm.USER_ID

						WHERE (bc.BOOK_CLASS_ID = @BookClassId OR @BookClassId='')
						AND BOOK_NAME LIKE ('%' + @BookName + '%')
						AND (mm.USER_ID = @BookKeeper  OR  @BookKeeper='')
						AND (CODE_ID = @BookStatus OR  @BookStatus='')
						ORDER BY bd.BOOK_BOUGHT_DATE DESC";

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

		public List<string> GetBookNameList()
		{
			List<string> bookNames = new List<string>();
			DataTable dt = new DataTable();
			string sql = @"SELECT DISTINCT bd.BOOK_NAME AS BookName FROM BOOK_DATA bd";
			using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
			{
				conn.Open();
				SqlCommand cmd = new SqlCommand(sql, conn);
				SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
				sqlAdapter.Fill(dt);
				foreach (DataRow row in dt.Rows)
				{
					bookNames.Add(row["BookName"].ToString());
				}
				conn.Close();
			}

			return bookNames;
		}

		//boolean
		/// <summary>
		/// return true:success
		/// </summary>
		/// <param name="book"></param>
		/// <returns></returns>
		/// return bool
		public Boolean UpdateBook(Book book)
		{
			int success = 0;


			string sql = @"BEGIN TRY
							BEGIN TRANSACTION
							UPDATE BOOK_DATA
							SET BOOK_NAME = @BookName
							   ,BOOK_AUTHOR = @BookAuthor
							   ,BOOK_PUBLISHER = @BookPublisher
							   ,BOOK_NOTE = @BookNote
							   ,BOOK_BOUGHT_DATE = @BookBoughtDate
							   ,BOOK_CLASS_ID = @BookClassId
							   ,BOOK_STATUS = @BookStatus
							   ,BOOK_KEEPER = @BookKeeperId
							   ,MODIFY_DATE = @ModifyDate
							   ,MODIFY_USER = @ModifyUser
							WHERE BOOK_ID = @BookId
							COMMIT TRANSACTION
						END TRY
						BEGIN CATCH
							ROLLBACK TRANSACTION
						END CATCH";

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
				cmd.Parameters.Add(new SqlParameter("@BookId", book.BookId == null ? string.Empty : book.BookId));
				cmd.Parameters.Add(new SqlParameter("@ModifyDate", DateTime.Now));
				cmd.Parameters.Add(new SqlParameter("@ModifyUser", "admin"));
				success = cmd.ExecuteNonQuery();
				conn.Close();
			}

			if (success != 0)
			{
				return true;

			}
			else
			{
				return false;
			}

		}



		public Boolean CheckBookCanDelete(string bookId)
		{
			string sql = @"SELECT  BOOK_STATUS FROM BOOK_DATA
							WHERE BOOK_ID=@BookId";

			string status;
			using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
			{
				conn.Open();
				SqlCommand cmd = new SqlCommand(sql, conn);
				cmd.Parameters.Add(new SqlParameter("@BookId", bookId));
				status = cmd.ExecuteScalar().ToString();
				conn.Close();
			}

			if (status == "A")
			{
				return true;
			}
			else
			{
				return false;
			}

		}

		/// <summary>
		/// delete book by id
		/// </summary>
		public string DeleteBookById(string bookId)
		{
			int success = 0;

			if (!CheckBookCanDelete(bookId))
			{
				return "Borrowed";
			}


			// 此地方以後需判斷 到底是狀態/能不能刪除????
			string sql = @" BEGIN TRY
									BEGIN TRANSACTION
									DELETE FROM BOOK_DATA
									WHERE BOOK_ID = @BookId
										AND BOOK_STATUS = 'A' 
									COMMIT TRANSACTION
								END TRY
								BEGIN CATCH
									ROLLBACK TRANSACTION
								END CATCH";
			using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
			{
				conn.Open();
				SqlCommand cmd = new SqlCommand(sql, conn);
				cmd.Parameters.Add(new SqlParameter("@BookId", bookId));
				success = cmd.ExecuteNonQuery();
				conn.Close();
			}

			if (success > 0)
			{
				return "Success";
			}
			else
			{
				return "False";
			}


		}

		/// <summary>
		/// Map資料進List
		/// </summary>
		/// <param bookData="書本資料"></param>
		/// <returns></returns>

		private List<Model.Book> MapBookDataToList(DataTable bookData)
		{
			List<Model.Book> result = new List<Book>();
			foreach (DataRow row in bookData.Rows)
			{
				result.Add(new Book()
				{

					BookId = row["BookId"].ToString(),
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


		public Boolean CheckBookIdExist(string bookId)
		{
			string sql = @"SELECT 1                 
						   FROM BOOK_DATA bd
						   WHERE BOOK_ID = @BookId";

			using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
			{
				conn.Open();
				SqlCommand cmd = new SqlCommand(sql, conn);

				cmd.Parameters.Add(new SqlParameter("@BookId", bookId));
				int success = Convert.ToInt32(cmd.ExecuteScalar());
				conn.Close();
				if (success == 1)
				{
					return true;
				}
				else
				{
					return false;
				}

			}


		}


		/// <summary>
		/// Get book data (need to be edit) by id
		/// </summary>
		/// <param name="bookId"></param>
		/// <returns></returns>
		public Book GetBookEditDataById(string bookId)
		{



			DataTable dt = new DataTable();
			string sql = @"SELECT 
							bd.BOOK_ID AS BookId,
							bd.BOOK_NAME AS BookName,
							bd.BOOK_AUTHOR AS BookAuthor,
							bd.BOOK_PUBLISHER AS BookPublisher,
							bd.BOOK_NOTE AS BookNote,
							FORMAT(bd.BOOK_BOUGHT_DATE ,'yyyy/MM/dd') AS BookBoughtDate,
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

		/// <summary>
		/// Get book records by bookId
		/// </summary>
		/// <param name="bookId"></param>
		/// <returns></returns>
		public List<Book> GetBookRecordById(string bookId)
		{

			DataTable dt = new DataTable();
			string sql = @"SELECT
								FORMAT(blr.LEND_DATE, 'yyyy/MM/dd') AS LendDate
							   ,blr.KEEPER_ID AS KeeperId
							   ,mm.USER_ENAME AS UserEName
							   ,mm.USER_CNAME AS UserCName
							FROM BOOK_LEND_RECORD blr
							INNER JOIN MEMBER_M mm
								ON blr.KEEPER_ID = mm.USER_ID
							WHERE blr.BOOK_ID = @bookId";
			using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
			{
				conn.Open();

				SqlCommand cmd = new SqlCommand(sql, conn);
				cmd.Parameters.Add("@bookId", bookId);
				SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
				sqlAdapter.Fill(dt);
				conn.Close();
			}
			List<Book> books = new List<Book>();

			foreach (DataRow row in dt.Rows)
			{
				books.Add(new Book()
				{
					LendDate = row["LendDate"].ToString(),
					BookKeeperId = row["KeeperId"].ToString(),
					UserEName = row["UserEName"].ToString(),
					UserCName = row["UserCName"].ToString()
				}
				);
			}
			return books;
		}


	}

}
