namespace Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _201693 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Themes",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 32, fixedLength: true, unicode: false),
                        UserId = c.String(maxLength: 32, fixedLength: true, unicode: false),
                        StoreId = c.String(maxLength: 32, fixedLength: true, unicode: false),
                        NO = c.String(nullable: false, maxLength: 16, unicode: false),
                        Name = c.String(nullable: false, maxLength: 100, unicode: false),
                        GameMinute = c.Int(nullable: false),
                        CreatedTime = c.DateTime(nullable: false),
                        UpdatedTime = c.DateTime(nullable: false),
                        Flag = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            AlterColumn("dbo.Pays", "NO", c => c.String(nullable: false, maxLength: 16, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Pays", "NO", c => c.String(maxLength: 16, unicode: false));
            DropTable("dbo.Themes");
        }
    }
}
