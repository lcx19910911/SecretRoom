namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class User: BaseEntity
    {
        /// <summary>
        /// �˵�Ȩ�� 
        /// </summary>
        public Nullable<long> MenuFlag { get; set; }

        /// <summary>
        /// �ŵ�Ȩ�� 
        /// </summary>
        public Nullable<long> StoreFlag { get; set; }

        /// <summary>
        /// ������Id
        /// </summary>
        [Display(Name = "������Id")]
        [Column("CreaterId", TypeName = "char"), MaxLength(32)]
        public string CreaterId { get; set; }

        /// <summary>
        /// �˺�
        /// </summary>
        [Display(Name = "�˺�")]
        [MaxLength(12)]
        [Required(ErrorMessage ="��½�˺Ų���Ϊ��")]
        [Column("Account", TypeName = "varchar")]
        public string Account { get; set; }
        /// <summary>
        /// ��˾����
        /// </summary>
        [Display(Name = "��˾����")]
        [MaxLength(100)]
        [Required(ErrorMessage = "��˾���Ʋ���Ϊ��")]
        [Column("CompanyName", TypeName = "varchar")]
        public string CompanyName { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        [Display(Name = "����")]
        [MaxLength(128)]
        [Required(ErrorMessage = "���벻��Ϊ��")]
        [Column("Password", TypeName = "varchar")]
        public string Password { get; set; }
        /// <summary>
        /// �ֻ���
        /// </summary>
        [Display(Name = "�ֻ���")]
        [MaxLength(11)]
        [Required(ErrorMessage = "�ֻ��Ų���Ϊ��")]
        [RegularExpression(@"((\d{11})$)", ErrorMessage = "��ʽ����ȷ")]
        public string Mobile { get; set; }
        /// <summary>
        /// ��ע
        /// </summary>
        [Display(Name = "��ע")]
        [MaxLength(128)]
        [Column("Remark", TypeName = "varchar")]
        public string Remark { get; set; }

        /// <summary>
        /// ����������
        /// </summary>
        [Display(Name = "����������")]
        [MaxLength(12),MinLength(6)]
        [NotMapped]
        public string NewPassword { get; set; }

        /// <summary>
        /// �ٴ���������
        /// </summary>
        [Display(Name = "�ٴ���������")]
        [MaxLength(12), MinLength(6),Compare("NewPassword",ErrorMessage="�����������벻һ��")]
        [NotMapped]
        public string ConfirmPassword { get; set; }


        /// <summary>
        /// ����ʱ��
        /// </summary>
        [Display(Name = "�˺ŵ���ʱ��")]       
        public Nullable<System.DateTime> ExpireTime { get; set; }

        /// <summary>
        /// �Ƿ��ǹ���Ա
        /// </summary>
        public YesOrNoCode IsAdmin { get; set; }


        /// <summary>
        /// ����ƾ֤ͼƬ
        /// </summary>
        [Display(Name = "����ƾ֤ͼƬ")]
        [Column("PayImage", TypeName = "varchar"), MaxLength(300)]
        public string PayImage { get; set; }
    }
}
