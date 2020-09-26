using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Web;
using System.Web.Mvc;
using workshop4.Models;

namespace workshop4.Controllers
{
    public class DemoController : Controller
    {
        // GET: Demo
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost()]
        public JsonResult GetTestData(string testString)
        {
            return Json(testString + "ajax");
        }




        [HttpPost()]
        public JsonResult GetDropDownListData()
        {
            List<string> tempData =  new List<string>();
            tempData.Add("item 1");
            tempData.Add("item 1");
            tempData.Add("item 1");
            return Json(tempData);
        }



        [HttpPost()]
        public JsonResult GetGridListData()
        {
            List<Models.Demo> tempData = new List<Demo>();
            tempData.Add(new Demo("1","eric"));
            tempData.Add(new Demo("1", "eric"));
            tempData.Add(new Demo("1", "eric"));

            return Json(tempData);
        }


        [HttpPost()]
        public JsonResult GetAutoCompleteData()
        {
            List<string> tempData = new List<string>();
            tempData.Add("資料庫設計");
            tempData.Add("資料庫a");
            tempData.Add("資料庫b");
            tempData.Add("演算法");

            return Json(tempData);

        }
    }
}