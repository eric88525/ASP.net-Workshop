using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;

namespace workshop4.Models
{
    public class Book

    {

        public string BookId { get; set; }

        [DisplayName("書名")]
        [Required(ErrorMessage = "此欄位必填")]
        public string BookName { get; set; }

        [DisplayName("圖書類別")]
        [Required(ErrorMessage = "此欄位必填")]
        public string BookClassId { get; set; }

        [DisplayName("作者")]
       // [Required(ErrorMessage = "此欄位必填")]
        public string BookAuthor { get; set; }

        [DisplayName("購書日期")]
        [Required(ErrorMessage = "此欄位必填")]


        [DataType(DataType.Date)]
        
        public string BookBoughtDate { get; set; }

        [DisplayName("出版商")]
        //[Required(ErrorMessage = "此欄位必填")]
        public string BookPublisher { get; set; }
        [DisplayName("內容簡介")]
        //[Required(ErrorMessage = "此欄位必填")]
        public string BookNote { get; set; }




        // for edit
        /// <summary>
        /// 中文-英文
        /// </summary>
        ///     
        [DisplayName("借閱人")]
        public string BookKeeperId { get; set; }  // keeper id
        public string BookAmount { get; set; }
        public string CreateDate { get; set; }
        public string CreateUser { get; set; }
        public string ModifyDate { get; set; }
        public string ModifyUser { get; set; }


        // for search result
        // class name
        public string BookClass { get; set; }
        [DisplayName("借閱狀態")]
        [Required(ErrorMessage = "此欄位必填")]
        public string BookStatus { get; set; }
        public string UserEName { get; set; }
        public string UserCName { get; set; }

       
        public string UserFullName { get; set; }


    }
}