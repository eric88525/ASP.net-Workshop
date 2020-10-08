using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManageSystem.Model
{
    public class BookArgs
    {

        public string BookName { get; set; }
        public string BookClassId { get; set; } // id
        public string BookKeeper { get; set; }
        public string BookStatus { get; set; }


    }
}
