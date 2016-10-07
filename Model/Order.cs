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
        /// ��˾Id
        /// </summary>
        [Display(Name = "��˾Id")]
        [Column("CompanyId", TypeName = "char"), MaxLength(32)]
        public string CompanyId { get; set; }

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
        public string SecondPayName { get; set; }

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
        /// �ڶ�֧������Id
        /// </summary>
        [Display(Name = "�ڶ�֧������Id")]
        [Column("SecondPayId", TypeName = "char"), MaxLength(32)]
        public string SecondPayId { get; set; }

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
        /// ���
        /// </summary>
        [Display(Name = "���")]
        public Nullable<decimal> SecondMoney { get; set; }

        /// <summary>
        /// �����ܶ�
        /// </summary>
        [Display(Name = "�����ܶ�")]
        public Nullable<decimal> DrinkMoney { get; set; }


        /// <summary>
        /// ���ϵ�json�ַ���
        /// </summary>
        [Display(Name = "���ϵ�json�ַ���")]
        [MaxLength(512)]
        [Column("DrinkJsonStr", TypeName = "varchar")]
        public string DrinkJsonStr { get; set; }


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
        /// ԤԼʱ��
        /// </summary>
        [Display(Name = "ԤԼʱ��")]
        [Range(0, 59)]
        public int Minute { get; set; }

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


    /// <summary>
    /// ����
    /// </summary>
    public  class OrderExecle 
    {


        /// <summary>
        /// ����Id
        /// </summary>
        public string PayName { get; set; }

        /// <summary>
        /// ����Id
        /// </summary>
        public string SecondPayName { get; set; }
        
        /// <summary>
        /// ����Id
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// ����Id
        /// </summary>
        public string StoreName { get; set; }

        /// <summary>
        /// ����Id
        /// </summary>
        public string ThemeName { get; set; }

        /// <summary>
        /// ���
        /// </summary>
        public Nullable<decimal> Money { get; set; }

        /// <summary>
        /// �����ܶ�
        /// </summary>
        [Display(Name = "�����ܶ�")]
        public Nullable<decimal> DrinkMoney { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        public int PeopleCount { get; set; }

        /// <summary>
        /// �ֻ���
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// ԤԼʱ��
        /// </summary>
        public int AppointmentTime { get; set; }


        /// <summary>
        /// �ܶ�
        /// </summary>
        public decimal AllMoney { get; set; }

        /// <summary>
        /// ��ʼʱ��
        /// </summary>
        public Nullable<DateTime> StartTime { get; set; }

        /// <summary>
        /// ����ʱ��
        public Nullable<DateTime> OverTime { get; set; }

        /// <summary>
        /// �Ƿ����
        /// </summary>
        public string IsPlay { get; set; }
    }
}
