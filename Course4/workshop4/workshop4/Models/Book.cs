using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace workshop4.Models
{
    public class Book

    {
        
        public string BookId { get; set; }

       // [DisplayName("書名")]
        public string BookName { get; set; }

        // [DisplayName("類別")]
        public string BookClassId { get; set; }

        // [DisplayName("作者")]
        public string BookAuthor { get; set; }

        //  [DisplayName("購買日期")]
        public string BookBoughtDate { get; set; }

        //  [DisplayName("生產者")]
        public string BookPublisher { get; set; }

        // [DisplayName("簡介")]
        public string BookNote { get; set; }


        public string BookStatus { get; set; }


        public string BookKeeper { get; set; }
        public string BookAmount { get; set; }
        public string CreateDate { get; set; }
        public string CreateUser { get; set; }
        public string ModifyDate { get; set; }
        public string ModifyUser { get; set; }



    }
}