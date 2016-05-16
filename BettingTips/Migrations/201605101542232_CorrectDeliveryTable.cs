namespace BettingTips.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CorrectDeliveryTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Deliveries", "DeliveryStatus", c => c.String(maxLength: 100));
            DropColumn("dbo.Deliveries", "Message");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Deliveries", "Message", c => c.String(maxLength: 1000));
            DropColumn("dbo.Deliveries", "DeliveryStatus");
        }
    }
}
