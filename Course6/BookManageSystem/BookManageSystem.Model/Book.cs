using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManageSystem.Model
{
    public class Book

    {

        public string BookId { get; set; }

        [DisplayName("書名")]
        public string BookName { get; set; }

        [DisplayName("圖書類別")]
        public string BookClassId { get; set; }

        [DisplayName("作者")]
        public string BookAuthor { get; set; }

        [DisplayName("購書日期")]
        public string BookBoughtDate { get; set; }

        [DisplayName("出版商")]
        public string BookPublisher { get; set; }
        [DisplayName("內容簡介")]
        public string BookNote { get; set; }

        // for edit
        /// <summary>
        /// 中文-英文
        /// </summary>
        [DisplayName("借閱人")]
        public string BookKeeperId { get; set; }  // keeper id
        public string BookAmount { get; set; }
        public string CreateDate { get; set; }
        public string CreateUser { get; set; }
        public string ModifyDate { get; set; }
        public string ModifyUser { get; set; }


        // for search result
        // class name
        [DisplayName("圖書類別")]
        public string BookClass { get; set; }

        [DisplayName("借閱狀態")]
        public string BookStatus { get; set; }
        public string UserEName { get; set; }
        public string UserCName { get; set; }


        public string UserFullName { get; set; }

        public string LendDate { get; set; }


    }
}
