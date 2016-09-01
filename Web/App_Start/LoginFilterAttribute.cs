using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Controllers;

namespace Web
{
    /// <summary>
    /// 过滤器
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class LoginFilterAttribute : ActionFilterAttribute
    {
        public List<Tuple<string, string>> allowAction
        {
            get
            {
                List<Tuple<string, string>> allowAction = new List<Tuple<string, string>>();
                return allowAction;
            }
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controller = filterContext.Controller as BaseController;


            var actionName = filterContext.RouteData.Values["Action"].ToString();
            var controllerName = filterContext.RouteData.Values["Controller"].ToString();
            var actionMethodList = filterContext.Controller.GetType().GetMethods();
            string requestUrl = filterContext.HttpContext.Request.Url.ToString();


            //判断页面是否需要登录
           if (allowAction.FirstOrDefault(x => x.Item1.Equals(controllerName, StringComparison.OrdinalIgnoreCase) && x.Item2.Equals(actionName, StringComparison.OrdinalIgnoreCase)) == null)
            {
                if (!CookieHelper.IsLogin())
                {
                    if (!controllerName.Equals("login", StringComparison.OrdinalIgnoreCase))
                    {
                        var actionMethod = actionMethodList.FirstOrDefault(x => x.Name.Equals(actionName, StringComparison.OrdinalIgnoreCase));
                        if (actionMethod != null)
                        {
                                RedirectResult redirectResult = new RedirectResult("/login/index?redirecturl=" + requestUrl);
                                filterContext.Result = redirectResult;
                        }
                    }

                }
            }
        }
    }
}