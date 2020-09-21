using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace workshop4.Models
{
    public class BookService
    {

        private string GetDBConnectionString()
        {
            return
                System.Configuration.ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString.ToString();
        }




    }
}