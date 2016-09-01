
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    /// <summary>
    /// 登录用户
    /// </summary>
    public class LoginUser
    {
        public LoginUser(User user)
        {
            this.ID = user.ID;
            this.Account = user.Account;
            this.Name = user.Name;
        }


        public LoginUser()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
    }
}
