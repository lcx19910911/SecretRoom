﻿using Core;
using Model;
using Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            LogHelper.WriteCustom(string.Format("Application_Start At {0} \r\n", DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss")), @"Application\", false);
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //脚本资源注册
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            ///预加载
            using (var dbcontext = new DbRepository())
            {
                dbcontext.Database.CreateIfNotExists();
                var objectContext = ((IObjectContextAdapter)dbcontext).ObjectContext;
                var mappingCollection = (StorageMappingItemCollection)objectContext.MetadataWorkspace.GetItemCollection(DataSpace.CSSpace);
                mappingCollection.GenerateViews(new List<EdmSchemaError>());
                if (!dbcontext.User.Any())
                {
                    dbcontext.User.Add(new Model.User()
                    {
                        ID = Guid.NewGuid().ToString("N"),
                        CreatedTime = DateTime.Now,
                        UpdatedTime = DateTime.Now,
                        CompanyId = Guid.NewGuid().ToString("N"),
                        Flag = (long)GlobalFlag.Normal,
                        Account = Params.AdminName,
                        Password = CryptoHelper.MD5_Encrypt(Params.Password),
                        MenuFlag = -1,
                        StoreFlag = -1,
                        CompanyName = Params.CompanyName,
                        IsAdmin=YesOrNoCode.Yes,
                        Name=Params.CompanyName,
                        CreaterId="1",
                        Mobile=Params.Mobile,
                        ExpireTime =DateTime.Now.AddYears(100)
                    });
                    dbcontext.SaveChanges();
                }
            }
        }

     

        protected void Application_End(object sender, EventArgs e)
        {
            var runtime = (HttpRuntime)typeof(HttpRuntime).InvokeMember("_theRuntime", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, null, null);
            if (runtime != null)
            {
                string shutDownMessage = (string)runtime.GetType().InvokeMember("_shutDownMessage", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField, null, runtime, null);
                string shutDownStack = (string)runtime.GetType().InvokeMember("_shutDownStack", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField, null, runtime, null);
                ApplicationShutdownReason shutDownReason = (ApplicationShutdownReason)runtime.GetType().InvokeMember("_shutdownReason", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField, null, runtime, null);
                LogHelper.WriteCustom(string.Format("Application_End:shutDownMessage:\r\n{0}\r\nshutDownStack:\r\n{1}\r\nshutDownReason:\r\n{2}\r\n", shutDownMessage, shutDownStack, shutDownReason.ToString()), @"Application\", false);
            }
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var exception = Server.GetLastError();
            if (exception is ThreadAbortException)
            {
                Thread.ResetAbort();
                HttpContext.Current.ClearError();
                return;
            }
            var httpException = exception as HttpException;

            if (httpException != null && httpException.GetHttpCode() == 404)
            {
                LogHelper.WriteCustom(httpException.ToString(), "404Error\\");
                Server.ClearError();
                Response.Clear();
                Response.Redirect("/base/_404");
            }
            else
            {
                LogHelper.WriteException("Application Error.", Server.GetLastError());
                Server.ClearError();
                Response.Clear();
                Response.Redirect("/base/_500");
            }

        }
    }
}
