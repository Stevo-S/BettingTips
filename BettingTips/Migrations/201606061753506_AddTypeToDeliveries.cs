namespace BettingTips.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTypeToDeliveries : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Deliveries", "Type", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Deliveries", "Type");
        }
    }
}
