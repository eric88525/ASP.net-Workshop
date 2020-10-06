using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace BookMamageSystem.Models
{
    public class BookArgs
    {

        [DisplayName("書名")]
        public string BookName { get; set; }
        [DisplayName("圖書類別")]
        public string BookClassId { get; set; } // id
        [DisplayName("借閱人")]
        public string BookKeeper { get; set; }
        [DisplayName("借閱狀態")]
        public string BookStatus { get; set; }


    }
}