namespace BettingTips.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddInboundMessages : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.InboundMessages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Message = c.String(),
                        ServiceId = c.String(),
                        ProductId = c.String(),
                        InDate = c.DateTime(nullable: false),
                        UpdateDescription = c.String(),
                        SyncOrderType = c.String(),
                        traceUniqueId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.InboundMessages");
        }
    }
}
