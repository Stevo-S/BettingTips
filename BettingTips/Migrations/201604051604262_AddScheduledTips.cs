namespace BettingTips.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddScheduledTips : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OutboundMessages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Source = c.String(),
                        Destination = c.String(),
                        Message = c.String(),
                        MessageDate = c.DateTime(nullable: false),
                        ServiceId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ScheduledTips",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TipNumber = c.Int(nullable: false),
                        Tip = c.String(),
                        DateScheduled = c.DateTime(nullable: false),
                        Destination = c.String(),
                        ServiceId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ScheduledTips");
            DropTable("dbo.OutboundMessages");
        }
    }
}
