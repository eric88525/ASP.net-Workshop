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
        /// 得到下拉選單列表
        /// </summary>
        /// <param name="selectType"> 下拉選單種類 </param>
        /// <returns></returns>
        public List<SelectListItem> GetCodeTable(string selectType)

        {
            DataTable dt = new DataTable();
            string sql = "";
            switch (selectType)
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
            return this.MapCodeData(dt, selectType);
        }
        /// <summary>
        /// Maping 代碼資料
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private List<SelectListItem> MapCodeData(DataTable dt, string selectType)
        {


            string value = "";


            switch (selectType)
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
                    value = selectType;
                    break;

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


    }
}