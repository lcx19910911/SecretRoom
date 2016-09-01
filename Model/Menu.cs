
namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Menu
    {
        public string UNID { get; set; }
        public string Name { get; set; }
        public Nullable<long> Sort { get; set; }
        public Nullable<long> LimitFlag { get; set; }
        public string Link { get; set; }
        public string ParentID { get; set; }
        public string ClassName { get; set; }
        public long Flag { get; set; }
        public System.DateTime UpdatedTime { get; set; }
        public System.DateTime CreatedTime { get; set; }
    }
}
