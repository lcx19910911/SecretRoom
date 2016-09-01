namespace Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Role
    {
        public string UNID { get; set; }
        public Nullable<long> RoleFlag { get; set; }
        public string Name { get; set; }
        public Nullable<long> LimitFlag { get; set; }
        public string Remark { get; set; }
        public long Flag { get; set; }
        public System.DateTime UpdatedTime { get; set; }
        public System.DateTime CreatedTime { get; set; }
    }
}
