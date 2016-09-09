
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
            this.CompanyId = user.CompanyId;
            this.IsAdmin = user.IsAdmin;
            this.MenuFlag = user.MenuFlag;
            this.StoreFlag = user.StoreFlag;
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
        /// 公司名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 是否管理员
        /// </summary>
        public string CompanyId { get; set; }

        /// <summary>
        /// 权限值
        /// </summary>
        public Nullable<long> MenuFlag { get; set; }

        public Nullable<long> StoreFlag { get; set; }

        public YesOrNoCode IsAdmin { get; set; }
    }
}
