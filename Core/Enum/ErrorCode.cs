using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public enum ErrorCode
    {
        #region 系统操作0-99之间
        /// <summary>
        /// 操作成功
        /// </summary>
        [Description("操作成功.")]
        sys_success = 0,

        /// <summary>
        /// 操作失败,请联系管理员
        /// </summary>
        [Description("服务器异常.")]
        sys_fail = 1,

        /// <summary>
        /// 参数值格式有误
        /// </summary>
        [Description("参数值格式有误.")]
        sys_param_format_error = 2,


        /// <summary>
        /// 授权码无效
        /// </summary>
        [Description("授权码无效.")]
        sys_token_invalid = 11,

        /// <summary>
        /// 用户角色权限不足
        /// </summary>
        [Description("用户角色权限不足.")]
        sys_user_role_error = 12,
        

        #endregion

        #region 数据库操作 100-199
        /// <summary>
        /// 无法找到对应主键Id
        /// </summary>
        [Description("无法找到对应主键Id.")]
        datadatabase_primarykey_not_found = 60,

        /// <summary>
        /// 名称已存在
        /// </summary>
        [Description("无法找到对应主键Id.")]
        datadatabase_name_had = 61,

        /// <summary>
        /// 版本号已存在
        /// </summary>
        [Description("版本号已存在.")]
        datadatabase_version_had = 62,

        /// <summary>
        /// 数据库连接失败
        /// </summary>
        [Description("数据库连接失败.")]
        datadatabase_connect_failed = 63,

        #endregion






        #region 用户

        /// <summary>
        /// 用户还未登录
        /// </summary>
        [Description("用户还未登录.")]
        user_login_error = 100,

        /// <summary>
        /// 用户已存在
        /// </summary>
        [Description("用户名已存在.")]
        user_name_already_exist = 101,

        /// <summary>
        /// 已过验证时间
        /// </summary>
        [Description("已过验证时间.")]
        verification_time_out = 104,

        /// <summary>
        /// 用户不存在
        /// </summary>
        [Description("用户不存在.")]
        user_not_exit= 105,

        /// <summary>
        /// 账号已过期
        /// </summary>
        [Description("账号已过期")]
        user_expire = 106,

        /// <summary>
        /// 账号已被禁用
        /// </summary>
        [Description("账号已被禁用")]
        user_disabled = 106,

        #endregion

    }
}
