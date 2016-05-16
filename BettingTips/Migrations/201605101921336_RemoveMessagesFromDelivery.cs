namespace BettingTips.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveMessagesFromDelivery : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Deliveries", "Message");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Deliveries", "Message", c => c.String(maxLength: 1000));
        }
    }
}
