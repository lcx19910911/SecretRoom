namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// ����
    /// </summary>
    public partial class Order : BaseEntity
    {

        /// <summary>
        /// �����û�Id
        /// </summary>
        [Display(Name = "�����û�Id")]
        [Column("CreaterId", TypeName = "char"), MaxLength(32)]
        public string CreaterId { get; set; }

        /// <summary>
        /// ����Id
        /// </summary>
        [Display(Name = "����Id")]
        [Column("StoreId", TypeName = "char"), MaxLength(32)]
        public string StoreId { get; set; }

        /// <summary>
        /// ����Id
        /// </summary>
        [NotMapped]
        public string PayName { get; set; }

        /// <summary>
        /// ����Id
        /// </summary>
        [NotMapped]
        public string CreaterName { get; set; }

        /// <summary>
        /// ����Id
        /// </summary>
        [NotMapped]
        public string CompanyName { get; set; }

        /// <summary>
        /// ����Id
        /// </summary>
        [NotMapped]
        public string StoreName { get; set; }

        /// <summary>
        /// ����Id
        /// </summary>
        [NotMapped]
        public string ThemeName { get; set; }

        /// <summary>
        /// ����Id
        /// </summary>
        [Display(Name = "����Id")]
        [Column("ThemeId", TypeName = "char"), MaxLength(32)]
        public string ThemeId { get; set; }

        /// <summary>
        /// ֧������Id
        /// </summary>
        [Display(Name = "֧������Id")]
        [Column("PayId", TypeName = "char"), MaxLength(32)]
        public string PayId { get; set; }

        /// <summary>
        /// ��ע
        /// </summary>
        [Display(Name = "��ע")]
        [MaxLength(218)]
        [Column("Remark", TypeName = "varchar")]
        public string Remark { get; set; }

        /// <summary>
        /// ���
        /// </summary>
        [Display(Name = "���")]
        public Nullable<decimal> Money { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        [Range(1,1000000)]
        public int PeopleCount { get; set; }

        /// <summary>
        /// �ֻ���
        /// </summary>
        [Display(Name = "�ֻ���")]
        [MaxLength(11)]
        [Required(ErrorMessage = "�ֻ��Ų���Ϊ��")]
        [RegularExpression(@"((\d{11})$)", ErrorMessage = "�ֻ���ʽ����ȷ")]
        public string Mobile { get; set; }

        /// <summary>
        /// ԤԼʱ��
        /// </summary>
        [Display(Name = "ԤԼʱ��")]
        [Required(ErrorMessage = "ԤԼʱ�䲻��Ϊ��")]
        public int AppointmentTime { get; set; }

        /// <summary>
        /// �ܶ�
        /// </summary>
        [Display(Name = "�ܶ�")]
        public decimal AllMoney { get; set; }

        /// <summary>
        /// ��ʼʱ��
        /// </summary>
        [Display(Name = "��ʼʱ��")]
        public Nullable<DateTime> StartTime { get; set; }

        /// <summary>
        /// ����ʱ��
        /// </summary>
        [Display(Name = "����ʱ��")]
        public Nullable<DateTime> OverTime { get; set; }

        /// <summary>
        /// �Ƿ����
        /// </summary>
        [Display(Name = "�Ƿ����")]
        [Required]
        public YesOrNoCode IsPlay { get; set; }
    }
}
