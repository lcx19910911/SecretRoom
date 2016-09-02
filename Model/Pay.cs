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
        public decimal RealMoney { get; set; }
    }
}
