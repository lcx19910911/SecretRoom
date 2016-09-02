namespace Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20160902 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Stores", "StartTime", c => c.String(nullable: false, maxLength: 16, unicode: false));
            AlterColumn("dbo.Stores", "EndTime", c => c.String(nullable: false, maxLength: 16, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Stores", "EndTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Stores", "StartTime", c => c.DateTime(nullable: false));
        }
    }
}
