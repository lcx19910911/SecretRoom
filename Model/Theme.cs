namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// 密室主题
    /// </summary>
    public partial class Theme : BaseEntity
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
        /// 密室主题编码
        /// </summary>
        [Display(Name = "密室主题编码")]
        [Column("NO", TypeName = "varchar"),MaxLength(16)]
        [Required(ErrorMessage = "密室主题编码不能为空")]
        public string NO { get; set; }

        /// <summary>
        /// 密室主题名称
        /// </summary>
        [Display(Name = "密室主题名称")]
        [MaxLength(100)]
        [Required(ErrorMessage = "密室主题名称不能为空")]
        [Column("Name", TypeName = "varchar")]
        public string Name { get; set; }


        /// <summary>
        /// 游戏时间
        /// </summary>
        [Display(Name = "游戏时间")]
        [Range(1, 600)]
        public int GameMinute { get; set; }

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
