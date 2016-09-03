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
    public class OrderController : BaseController
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
       
    }
}