namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class User: BaseEntity
    {
        /// <summary>
        /// 菜单权限 
        /// </summary>
        public Nullable<long> MenuFlag { get; set; }

        /// <summary>
        /// 门店权限 
        /// </summary>
        public Nullable<long> StoreFlag { get; set; }

        /// <summary>
        /// 创建者Id
        /// </summary>
        [Display(Name = "创建者Id")]
        [Column("CreaterId", TypeName = "char"), MaxLength(32)]
        public string CreaterId { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        [Display(Name = "账号")]
        [MaxLength(12)]
        [Required(ErrorMessage ="登陆账号不能为空")]
        [Column("Account", TypeName = "varchar")]
        public string Account { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>
        [Display(Name = "公司名称")]
        [MaxLength(100)]
        [Required(ErrorMessage = "公司名称不能为空")]
        [Column("CompanyName", TypeName = "varchar")]
        public string CompanyName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [Display(Name = "密码")]
        [MaxLength(128)]
        [Required(ErrorMessage = "密码不能为空")]
        [Column("Password", TypeName = "varchar")]
        public string Password { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        [Display(Name = "手机号")]
        [MaxLength(11)]
        [Required(ErrorMessage = "手机号不能为空")]
        [RegularExpression(@"((\d{11})$)", ErrorMessage = "格式不正确")]
        public string Mobile { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "备注")]
        [MaxLength(128)]
        [Column("Remark", TypeName = "varchar")]
        public string Remark { get; set; }

        /// <summary>
        /// 请输入密码
        /// </summary>
        [Display(Name = "请输入密码")]
        [MaxLength(12),MinLength(6)]
        [NotMapped]
        public string NewPassword { get; set; }

        /// <summary>
        /// 再次输入密码
        /// </summary>
        [Display(Name = "再次输入密码")]
        [MaxLength(12), MinLength(6),Compare("NewPassword",ErrorMessage="两次密码输入不一致")]
        [NotMapped]
        public string ConfirmPassword { get; set; }


        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "账号到期时间")]       
        public Nullable<System.DateTime> ExpireTime { get; set; }

        /// <summary>
        /// 是否是管理员
        /// </summary>
        public YesOrNoCode IsAdmin { get; set; }


        /// <summary>
        /// 付账凭证图片
        /// </summary>
        [Display(Name = "付账凭证图片")]
        [Column("PayImage", TypeName = "varchar"), MaxLength(300)]
        public string PayImage { get; set; }
    }
}
