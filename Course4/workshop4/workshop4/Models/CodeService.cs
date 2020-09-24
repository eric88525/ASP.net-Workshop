using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace workshop4.Models
{
    public class CodeService
    {


        /// <summary>
        /// 取得DB連線字串
        /// </summary>
        /// <returns></returns>
        private string GetDBConnectionString()
        {
            return
                System.Configuration.ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString.ToString();
        }
        /// <summary>
        /// 123
        /// </summary>
        /// <param name="column"> abc </param>
        /// <returns></returns>
        public List<SelectListItem> GetCodeTable(string column)

        {
            DataTable dt = new DataTable();
            string sql = "";
            switch (column)
            {
                case "BookClass":
                    sql = @"SELECT DISTINCT bc.BOOK_CLASS_NAME AS BookClass,bc.BOOK_CLASS_ID AS BookClassId  FROM BOOK_CLASS bc";
                    break;

                case "UserName":
                    sql = @"SELECT DISTINCT USER_ENAME AS UserName  FROM MEMBER_M mm";
                    break;

                case "BookStatus":
                    sql = @"SELECT DISTINCT bc.CODE_NAME BookStatus ,bc.CODE_ID  AS BookStatusId FROM BOOK_CODE bc WHERE bc.CODE_TYPE_DESC = '書籍狀態'";
                    break;

                case "KeeperFullName":
                    sql =
                        @"SELECT mm.USER_ID AS KeeperId,mm.USER_ENAME+'-'+mm.USER_CNAME AS KeeperFullName  FROM MEMBER_M mm";
                    break;
                    
                default:
                    return null; 
            }
           

            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }
            return this.MapCodeData(dt,column);
        }
        /// <summary>
        /// Maping 代碼資料
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private List<SelectListItem> MapCodeData(DataTable dt, string column)
        {


            string value = "";


            switch (column)
            {
                case "KeeperFullName":
                    value = "KeeperId";
                    break; 

                case "BookStatus":
                    value = "BookStatusId";
                    break;
                case "BookClass":
                    value = "BookClassId";
                    break;
                default:
                    value = column;
                    break;

            }



            List<SelectListItem> result = new List<SelectListItem>();
            foreach (DataRow row in dt.Rows)
            {
                result.Add(new SelectListItem()
                {
                    Text = row[column].ToString(),
                    Value = row[value].ToString()
                });
            }
            return result;
        }


    }
}