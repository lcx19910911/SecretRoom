namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// 门店
    /// </summary>
    public partial class Store : BaseEntity
    {
        /// <summary>
        /// 自身 未移值 
        /// </summary>
        public long LimitFlag { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        [Display(Name = "用户Id")]
        [Column("UserId", TypeName = "char"), MaxLength(32)]
        public string UserId { get; set; }

        /// <summary>
        /// 公司Id
        /// </summary>
        [Display(Name = "公司Id")]
        [Column("CompanyId", TypeName = "char"), MaxLength(32)]
        public string CompanyId { get; set; }

        /// <summary>
        /// Logo图片
        /// </summary>
        [Display(Name = "Logo图片")]
        [Column("Logo", TypeName = "varchar"),MaxLength(300)]
        public string Logo { get; set; }

        /// <summary>
        /// 密室名称
        /// </summary>
        [Display(Name = "密室名称")]
        [MaxLength(100)]
        [Required(ErrorMessage = "密室名称不能为空")]
        [Column("Name", TypeName = "varchar")]
        public string Name { get; set; }

        /// <summary>
        /// 营业开始时间
        /// </summary>
        [Display(Name = "营业开始时间")]
        [Required(ErrorMessage = "营业开始时间不能为空")]
        [Column("StartTime", TypeName = "varchar"), MaxLength(16)]
        public string StartTime { get; set; }

        /// <summary>
        /// 营业结束时间
        /// </summary>
        [Display(Name = "营业结束时间")]
        [Required(ErrorMessage = "营业结束时间不能为空")]
        [Column("EndTime", TypeName = "varchar"),MaxLength(16)]
        public string EndTime { get; set; }

        /// <summary>
        /// 订单间隔时间
        /// </summary>
        //[Display(Name = "订单间隔时间")]
        //[Range(1, 600)]
        //public int OrderSpaceMinute { get; set; }


        /// <summary>
        /// 手机号
        /// </summary>
        [Display(Name = "手机号")]
        [MaxLength(11)]
        [Required(ErrorMessage = "手机号不能为空")]
        [RegularExpression(@"((\d{11})$)", ErrorMessage = "手机格式不正确")]
        public string Mobile { get; set; }


        /// <summary>
        /// 省份
        /// </summary>
        [Display(Name = "省份")]
        [MaxLength(32)]
        [Required(ErrorMessage = "省份不能为空")]
        public string Province { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        [Display(Name = "城市")]
        [MaxLength(32)]
        [Required(ErrorMessage = "城市不能为空")]
        public string City { get; set; }
    }
}
