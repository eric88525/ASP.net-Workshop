using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookMamageSystem.Models
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
        /// 得到下拉選單列表
        /// </summary>
        /// <param name="selectType"> 下拉選單種類 </param>
        /// <returns></returns>
        public List<SelectListItem> GetCodeTable(string selectType)

        {
            DataTable dt = new DataTable();
            string sql = "";
            string value = "";
            switch (selectType)
            {
                // remove distinct 
                case "BookClass":
                    sql = @"SELECT  bc.BOOK_CLASS_NAME AS BookClass,bc.BOOK_CLASS_ID AS BookClassId  FROM BOOK_CLASS bc";
                    value = "BookClassId";
                    break;

                case "UserName":
                    sql = @"SELECT  USER_ENAME AS UserName ,USER_ID as UserId FROM MEMBER_M mm";
                    value = "UserId";
                    break;

                case "BookStatus":
                    sql = @"SELECT  bc.CODE_NAME BookStatus ,bc.CODE_ID  AS BookStatusId FROM BOOK_CODE bc WHERE bc.CODE_TYPE = 'BOOK_STATUS'";
                    value = "BookStatusId";
                    break;

                case "KeeperFullName":
                    sql =
                        @"SELECT mm.USER_ID AS KeeperId,mm.USER_ENAME+'-'+mm.USER_CNAME AS KeeperFullName  FROM MEMBER_M mm";
                    value = "KeeperId";
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

            List<SelectListItem> result = new List<SelectListItem>();
            foreach (DataRow row in dt.Rows)
            {
                result.Add(new SelectListItem()
                {
                    Text = row[selectType].ToString(),
                    Value = row[value].ToString()
                });
            }
            return result;



           
        }
        /// <summary>
        /// Maping 代碼資料
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>


        // 再加一個參數 value 避免耦合度太高!或上去
     /*   private List<SelectListItem> MapCodeData(DataTable dt, string selectType, string value)
        {

            List<SelectListItem> result = new List<SelectListItem>();
            foreach (DataRow row in dt.Rows)
            {
                result.Add(new SelectListItem()
                {
                    Text = row[selectType].ToString(),
                    Value = row[value].ToString()
                });
            }
            return result;
        }*/


    }
}