using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    /// <summary>
    /// 目录权限
    /// </summary>
    public enum MenuFlag
    {
        /// <summary>
        /// 在线订单
        /// </summary>
        [Description("在线订单")]
        Order =1,

        /// <summary>
        /// 主题管理
        /// </summary>
        [Description("主题管理")]
        Theme = 2,

        /// <summary>
        /// 密室管理
        /// </summary>
        [Description("密室管理")]
        Room = 4,

        /// <summary>
        /// 统计报表
        /// </summary>
        [Description("统计报表")]
        Report = 8,

        /// <summary>
        /// 支付方式
        /// </summary>
        [Description("支付方式")]
        Pay = 16,

        /// <summary>
        /// 用户管理
        /// </summary>
        [Description("用户管理")]
        User = 32,
    }
}
