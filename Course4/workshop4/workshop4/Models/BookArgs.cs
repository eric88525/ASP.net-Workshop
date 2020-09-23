using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace workshop4.Models
{
    public class BookArgs
    {

        //  [DisplayName("書名")]
        public string BookName { get; set; }
        //  [DisplayName("類別")]
        // id
        public string BookClassId { get; set; }
        //  [DisplayName("借閱人")]
        public string BookKeeper { get; set; }
        //  [DisplayName("借閱狀態")]
        public string BookStatus { get; set; }


    }
}