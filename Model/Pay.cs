namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// ֧������
    /// </summary>
    public partial class Pay : BaseEntity
    {

        /// <summary>
        /// �û�Id
        /// </summary>
        [Display(Name = "�û�Id")]
        [Column("UserId", TypeName = "char"), MaxLength(32)]
        public string UserId { get; set; }

        /// <summary>
        /// ����Id
        /// </summary>
        [Display(Name = "����Id")]
        [Column("StoreId", TypeName = "char"), MaxLength(32)]
        public string StoreId { get; set; }

        /// <summary>
        /// ֧�����ͱ���
        /// </summary>
        [Display(Name = "֧�����ͱ���")]
        [Column("NO", TypeName = "varchar"),MaxLength(16)]
        public string NO { get; set; }

        /// <summary>
        /// ֧����������
        /// </summary>
        [Display(Name = "֧����������")]
        [MaxLength(100)]
        [Required(ErrorMessage = "֧���������Ʋ���Ϊ��")]
        [Column("Name", TypeName = "varchar")]
        public string Name { get; set; }

        /// <summary>
        /// ʵ�ʽ��
        /// </summary>
        [Display(Name = "ʵ�ʽ��")]
        public decimal RealMoney { get; set; }
    }
}
