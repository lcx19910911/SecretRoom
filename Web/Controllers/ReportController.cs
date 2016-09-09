using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    /// <summary>
    /// 密室
    /// </summary>
    [LoginFilter]
    public class ReportController : BaseController
    {
        // GET: 
        public ActionResult Index()
        {
            return View(WebService.Get_StoreSelectItem(""));
        }

        public ActionResult GetReport(int index, string storeId, DateTime start, DateTime end)
        {
            return JResult(WebService.GetReport(index, storeId, start, end));
        }

    }
}