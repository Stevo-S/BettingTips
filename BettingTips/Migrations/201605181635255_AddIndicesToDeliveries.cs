namespace BettingTips.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIndicesToDeliveries : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Deliveries", "Destination");
            CreateIndex("dbo.Deliveries", "DeliveryStatus");
            CreateIndex("dbo.Deliveries", "TimeStamp");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Deliveries", new[] { "TimeStamp" });
            DropIndex("dbo.Deliveries", new[] { "DeliveryStatus" });
            DropIndex("dbo.Deliveries", new[] { "Destination" });
        }
    }
}
