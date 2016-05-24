namespace BettingTips.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddWebRequests : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WebRequests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(maxLength: 50),
                        Content = c.String(),
                        Timestamp = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Type)
                .Index(t => t.Timestamp);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.WebRequests", new[] { "Timestamp" });
            DropIndex("dbo.WebRequests", new[] { "Type" });
            DropTable("dbo.WebRequests");
        }
    }
}
