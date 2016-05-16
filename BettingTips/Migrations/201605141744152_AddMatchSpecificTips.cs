namespace BettingTips.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMatchSpecificTips : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MatchSpecificTips",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Tip = c.String(maxLength: 900),
                        SendTime = c.DateTime(nullable: false),
                        Expiration = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.SendTime)
                .Index(t => t.Expiration);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.MatchSpecificTips", new[] { "Expiration" });
            DropIndex("dbo.MatchSpecificTips", new[] { "SendTime" });
            DropTable("dbo.MatchSpecificTips");
        }
    }
}
