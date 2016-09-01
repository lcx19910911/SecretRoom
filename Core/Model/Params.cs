using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Web.Security;

namespace Core
{
    public class Params
    {

        /// <summary>
        /// 密钥
        /// </summary>
        public static readonly string SecretKey = ConfigurationManager.AppSettings["SecretKey"];

        /// <summary>
        /// 登陆cookie
        /// </summary>
        public static readonly string UserCookieName = "site_user";

        /// <summary>
        /// cookie 过期时间
        /// </summary>
        public static readonly int CookieExpires = 120;
    }
}
