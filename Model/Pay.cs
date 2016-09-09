namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// 支付类型
    /// </summary>
    public partial class Pay : BaseEntity
    {

        /// <summary>
        /// 公司Id
        /// </summary>
        [Display(Name = "公司Id")]
        [Column("CompanyId", TypeName = "char"), MaxLength(32)]
        public string CompanyId { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        [Display(Name = "用户Id")]
        [Column("UserId", TypeName = "char"), MaxLength(32)]
        public string UserId { get; set; }

        /// <summary>
        /// 密室Id
        /// </summary>
        [Display(Name = "密室Id")]
        [Column("StoreId", TypeName = "char"), MaxLength(32)]
        public string StoreId { get; set; }

        /// <summary>
        /// 支付类型编码
        /// </summary>
        [Display(Name = "支付类型编码")]
        [Column("NO", TypeName = "varchar"),MaxLength(16)]
        [Required(ErrorMessage = "支付类型编码不能为空")]
        public string NO { get; set; }

        /// <summary>
        /// 支付类型名称
        /// </summary>
        [Display(Name = "支付类型名称")]
        [MaxLength(100)]
        [Required(ErrorMessage = "支付类型名称不能为空")]
        [Column("Name", TypeName = "varchar")]
        public string Name { get; set; }

        /// <summary>
        /// 实际金额
        /// </summary>
        [Display(Name = "实际金额")]
        [Required(ErrorMessage = "实际金额不能为空")]
        public decimal RealMoney { get; set; }

        /// <summary>
        /// 迷失名称
        /// </summary>
        [NotMapped]
        public string StoreName { get; set; }


        /// <summary>
        /// 状态
        /// </summary>
        [NotMapped]
        public string State { get; set; }
    }
}
