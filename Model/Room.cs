namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// ����
    /// </summary>
    public partial class Room : BaseEntity
    {

        /// <summary>
        /// ��˾����
        /// </summary>
        [Display(Name = "��������")]
        [MaxLength(100)]
        [Required(ErrorMessage = "�������Ʋ���Ϊ��")]
        [Column("Name", TypeName = "varchar")]
        public string Name { get; set; }

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
        /// ״̬
        /// </summary>
        public long Flag { get; set; }

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
        public System.DateTime ExpireTime { get; set; }

        /// <summary>
        /// �Ƿ��ǹ���Ա
        /// </summary>
        public YesOrNoCode IsAdmin { get; set; }

    }
}
