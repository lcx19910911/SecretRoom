namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// ��������
    /// </summary>
    public partial class Theme : BaseEntity
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
        /// �����������
        /// </summary>
        [Display(Name = "�����������")]
        [Column("NO", TypeName = "varchar"),MaxLength(16)]
        [Required(ErrorMessage = "����������벻��Ϊ��")]
        public string NO { get; set; }

        /// <summary>
        /// ������������
        /// </summary>
        [Display(Name = "������������")]
        [MaxLength(100)]
        [Required(ErrorMessage = "�����������Ʋ���Ϊ��")]
        [Column("Name", TypeName = "varchar")]
        public string Name { get; set; }


        /// <summary>
        /// ��Ϸʱ��
        /// </summary>
        [Display(Name = "��Ϸʱ��")]
        [Range(1, 600)]
        public int GameMinute { get; set; }

        /// <summary>
        /// ��ʧ����
        /// </summary>
        [NotMapped]
        public string StoreName { get; set; }


        /// <summary>
        /// ״̬
        /// </summary>
        [NotMapped]
        public string State { get; set; }
    }
}
