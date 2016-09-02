﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using Core;

namespace Web.Controllers
{
    public class AccoutController : BaseController
    {
        // GET: Login
        public ActionResult Login()
        {
            return View();
        }


        /// <summary>
        /// 登录提交
        /// </summary>
        /// <param name="account">账号</param>
        /// <param name="password">密码</param> 
        /// <returns></returns>
        public JsonResult Submit(string account, string password)
        {
            return JResult(WebService.Login(account, password));
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        public ActionResult Quit()
        {
            CookieHelper.ClearCookie();
            return View("Login");
        }
    }
}