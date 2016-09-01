namespace Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class User
    {
        public string ID { get; set; }
        public Nullable<long> RoleFlag { get; set; }
        public string Account { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Mobile { get; set; }
        public string Remark { get; set; }
        public long Flag { get; set; }
        public System.DateTime UpdatedTime { get; set; }
        public System.DateTime CreatedTime { get; set; }
        public string TargetID { get; set; }
        public Nullable<int> TargetCode { get; set; }
    }
}
