namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// �ŵ�
    /// </summary>
    public partial class Store : BaseEntity
    {
        /// <summary>
        /// ���� δ��ֵ 
        /// </summary>
        public long LimitFlag { get; set; }

        /// <summary>
        /// �û�Id
        /// </summary>
        [Display(Name = "�û�Id")]
        [Column("UserId", TypeName = "char"), MaxLength(32)]
        public string UserId { get; set; }

        /// <summary>
        /// ��˾Id
        /// </summary>
        [Display(Name = "��˾Id")]
        [Column("CompanyId", TypeName = "char"), MaxLength(32)]
        public string CompanyId { get; set; }

        /// <summary>
        /// LogoͼƬ
        /// </summary>
        [Display(Name = "LogoͼƬ")]
        [Column("Logo", TypeName = "varchar"),MaxLength(300)]
        public string Logo { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        [Display(Name = "��������")]
        [MaxLength(100)]
        [Required(ErrorMessage = "�������Ʋ���Ϊ��")]
        [Column("Name", TypeName = "varchar")]
        public string Name { get; set; }

        /// <summary>
        /// Ӫҵ��ʼʱ��
        /// </summary>
        [Display(Name = "Ӫҵ��ʼʱ��")]
        [Required(ErrorMessage = "Ӫҵ��ʼʱ�䲻��Ϊ��")]
        [Column("StartTime", TypeName = "varchar"), MaxLength(16)]
        public string StartTime { get; set; }

        /// <summary>
        /// Ӫҵ����ʱ��
        /// </summary>
        [Display(Name = "Ӫҵ����ʱ��")]
        [Required(ErrorMessage = "Ӫҵ����ʱ�䲻��Ϊ��")]
        [Column("EndTime", TypeName = "varchar"),MaxLength(16)]
        public string EndTime { get; set; }

        /// <summary>
        /// �������ʱ��
        /// </summary>
        //[Display(Name = "�������ʱ��")]
        //[Range(1, 600)]
        //public int OrderSpaceMinute { get; set; }


        /// <summary>
        /// �ֻ���
        /// </summary>
        [Display(Name = "�ֻ���")]
        [MaxLength(11)]
        [Required(ErrorMessage = "�ֻ��Ų���Ϊ��")]
        [RegularExpression(@"((\d{11})$)", ErrorMessage = "�ֻ���ʽ����ȷ")]
        public string Mobile { get; set; }


        /// <summary>
        /// ʡ��
        /// </summary>
        [Display(Name = "ʡ��")]
        [MaxLength(32)]
        [Required(ErrorMessage = "ʡ�ݲ���Ϊ��")]
        public string Province { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        [Display(Name = "����")]
        [MaxLength(32)]
        [Required(ErrorMessage = "���в���Ϊ��")]
        public string City { get; set; }
    }
}
