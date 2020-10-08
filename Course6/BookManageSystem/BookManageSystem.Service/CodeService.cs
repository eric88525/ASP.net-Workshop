using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BookManageSystem.Service
{
    public class CodeService
    {
        BookManageSystem.dao.CodeDao codeDao = new BookManageSystem.dao.CodeDao();
        public List<SelectListItem> GetCodeTable(string selectType)
        {
            return codeDao.GetCodeTable(selectType);
        }

    }
}
