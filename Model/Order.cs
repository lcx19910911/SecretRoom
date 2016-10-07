namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// 订单
    /// </summary>
    public partial class Order : BaseEntity
    {

        /// <summary>
        /// 创建用户Id
        /// </summary>
        [Display(Name = "创建用户Id")]
        [Column("CreaterId", TypeName = "char"), MaxLength(32)]
        public string CreaterId { get; set; }

        /// <summary>
        /// 公司Id
        /// </summary>
        [Display(Name = "公司Id")]
        [Column("CompanyId", TypeName = "char"), MaxLength(32)]
        public string CompanyId { get; set; }

        /// <summary>
        /// 密室Id
        /// </summary>
        [Display(Name = "密室Id")]
        [Column("StoreId", TypeName = "char"), MaxLength(32)]
        public string StoreId { get; set; }

        /// <summary>
        /// 密室Id
        /// </summary>
        [NotMapped]
        public string PayName { get; set; }

        /// <summary>
        /// 密室Id
        /// </summary>
        [NotMapped]
        public string SecondPayName { get; set; }

        /// <summary>
        /// 密室Id
        /// </summary>
        [NotMapped]
        public string CreaterName { get; set; }

        /// <summary>
        /// 密室Id
        /// </summary>
        [NotMapped]
        public string CompanyName { get; set; }

        /// <summary>
        /// 密室Id
        /// </summary>
        [NotMapped]
        public string StoreName { get; set; }

        /// <summary>
        /// 密室Id
        /// </summary>
        [NotMapped]
        public string ThemeName { get; set; }

        /// <summary>
        /// 主题Id
        /// </summary>
        [Display(Name = "主题Id")]
        [Column("ThemeId", TypeName = "char"), MaxLength(32)]
        public string ThemeId { get; set; }

        /// <summary>
        /// 支付类型Id
        /// </summary>
        [Display(Name = "支付类型Id")]
        [Column("PayId", TypeName = "char"), MaxLength(32)]
        public string PayId { get; set; }


        /// <summary>
        /// 第二支付类型Id
        /// </summary>
        [Display(Name = "第二支付类型Id")]
        [Column("SecondPayId", TypeName = "char"), MaxLength(32)]
        public string SecondPayId { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "备注")]
        [MaxLength(218)]
        [Column("Remark", TypeName = "varchar")]
        public string Remark { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        [Display(Name = "金额")]
        public Nullable<decimal> Money { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        [Display(Name = "金额")]
        public Nullable<decimal> SecondMoney { get; set; }

        /// <summary>
        /// 饮料总额
        /// </summary>
        [Display(Name = "饮料总额")]
        public Nullable<decimal> DrinkMoney { get; set; }


        /// <summary>
        /// 饮料的json字符串
        /// </summary>
        [Display(Name = "饮料的json字符串")]
        [MaxLength(512)]
        [Column("DrinkJsonStr", TypeName = "varchar")]
        public string DrinkJsonStr { get; set; }


        /// <summary>
        /// 人数
        /// </summary>
        [Range(1,1000000)]
        public int PeopleCount { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        [Display(Name = "手机号")]
        [MaxLength(11)]
        [Required(ErrorMessage = "手机号不能为空")]
        [RegularExpression(@"((\d{11})$)", ErrorMessage = "手机格式不正确")]
        public string Mobile { get; set; }

        /// <summary>
        /// 预约时间
        /// </summary>
        [Display(Name = "预约时间")]
        [Required(ErrorMessage = "预约时间不能为空")]
        public int AppointmentTime { get; set; }



        /// <summary>
        /// 预约时间
        /// </summary>
        [Display(Name = "预约时间")]
        [Range(0, 59)]
        public int Minute { get; set; }

        /// <summary>
        /// 总额
        /// </summary>
        [Display(Name = "总额")]
        public decimal AllMoney { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        [Display(Name = "开始时间")]
        public Nullable<DateTime> StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        [Display(Name = "结束时间")]
        public Nullable<DateTime> OverTime { get; set; }

        /// <summary>
        /// 是否玩过
        /// </summary>
        [Display(Name = "是否玩过")]
        [Required]
        public YesOrNoCode IsPlay { get; set; }
    }


    /// <summary>
    /// 订单
    /// </summary>
    public  class OrderExecle 
    {


        /// <summary>
        /// 密室Id
        /// </summary>
        public string PayName { get; set; }

        /// <summary>
        /// 密室Id
        /// </summary>
        public string SecondPayName { get; set; }
        
        /// <summary>
        /// 密室Id
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// 密室Id
        /// </summary>
        public string StoreName { get; set; }

        /// <summary>
        /// 密室Id
        /// </summary>
        public string ThemeName { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public Nullable<decimal> Money { get; set; }

        /// <summary>
        /// 饮料总额
        /// </summary>
        [Display(Name = "饮料总额")]
        public Nullable<decimal> DrinkMoney { get; set; }

        /// <summary>
        /// 人数
        /// </summary>
        public int PeopleCount { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 预约时间
        /// </summary>
        public int AppointmentTime { get; set; }


        /// <summary>
        /// 总额
        /// </summary>
        public decimal AllMoney { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public Nullable<DateTime> StartTime { get; set; }

        /// <summary>
        /// 结束时间
        public Nullable<DateTime> OverTime { get; set; }

        /// <summary>
        /// 是否玩过
        /// </summary>
        public string IsPlay { get; set; }
    }
}
