namespace Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2016931 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Name", c => c.String(nullable: false, maxLength: 100, unicode: false));
            AddColumn("dbo.Users", "CompanyId", c => c.String(maxLength: 32, fixedLength: true, unicode: false));
            AlterColumn("dbo.Users", "CompanyName", c => c.String(maxLength: 128, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "CompanyName", c => c.String(nullable: false, maxLength: 100, unicode: false));
            DropColumn("dbo.Users", "CompanyId");
            DropColumn("dbo.Users", "Name");
        }
    }
}
