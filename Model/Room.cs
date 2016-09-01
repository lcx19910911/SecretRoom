namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// 密室
    /// </summary>
    public partial class Room : BaseEntity
    {

        /// <summary>
        /// 公司名称
        /// </summary>
        [Display(Name = "密室名称")]
        [MaxLength(100)]
        [Required(ErrorMessage = "密室名称不能为空")]
        [Column("Name", TypeName = "varchar")]
        public string Name { get; set; }

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
        /// 状态
        /// </summary>
        public long Flag { get; set; }

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
        public System.DateTime ExpireTime { get; set; }

        /// <summary>
        /// 是否是管理员
        /// </summary>
        public YesOrNoCode IsAdmin { get; set; }

    }
}
