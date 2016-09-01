using Core;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public partial class WebService
    {   
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public WebResult<bool> Login(string loginName, string password)
        {
            try
            {
                if (loginName.IsNullOrEmpty() || password.IsNullOrEmpty())
                {
                    return Result(false, ErrorCode.sys_param_format_error);
                }
                using (var db = new DbRepository())
                {
                    string md5Password = CryptoHelper.MD5_Encrypt(password);
                    var user = db.User.Where(x => x.Flag == (long)GlobalFlag.Normal&&x.Password == md5Password && x.Account.Equals(loginName)).FirstOrDefault();
                    if (user == null)
                        return Result(false, ErrorCode.user_not_exit);
                    else
                    {
                        CookieHelper.CreateCookie(user);
                        return Result(true);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteException(ex);
                return Result(false, ErrorCode.sys_fail);
            }
        }
    }
}
