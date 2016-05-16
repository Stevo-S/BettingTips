namespace BettingTips.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMessagesToDelivery : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Deliveries", "Message", c => c.String(maxLength: 1000));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Deliveries", "Message");
        }
    }
}
