using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Drink: BaseEntity
    {
        /// <summary>
        /// 饮料名称
        /// </summary>
        [Display(Name = "饮料名称")]
        [MaxLength(48)]
        [Required(ErrorMessage = "饮料名称不能为空")]
        [Column("Name", TypeName = "varchar")]
        public string Name { get; set; }


        /// <summary>
        /// 金额
        /// </summary>
        [Display(Name = "金额")]
        public decimal Money { get; set; }

        /// <summary>
        /// 密室Id
        /// </summary>
        [Display(Name = "密室Id")]
        [Column("StoreId", TypeName = "char"), MaxLength(32)]
        public string StoreId { get; set; }

        /// <summary>
        /// 公司Id
        /// </summary>
        [Display(Name = "公司Id")]
        [Column("CompanyId", TypeName = "char"), MaxLength(32)]
        public string CompanyId { get; set; }

        /// <summary>
        /// 密室Id
        /// </summary>
        [NotMapped]
        public string StoreName { get; set; }

        /// <summary>
        /// 密室Id
        /// </summary>
        [NotMapped]
        public string CompanyName { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        [NotMapped]
        public int Count { get; set; }
    }

    public class DrinkExecle
    {
        /// <summary>
        /// 饮料名称
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// 金额
        /// </summary>
        public decimal Money { get; set; }
         

        /// <summary>
        /// 密室Id
        /// </summary>
        public string AllName { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Count { get; set; }
    }
}
