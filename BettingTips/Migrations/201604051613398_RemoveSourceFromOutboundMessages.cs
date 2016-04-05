namespace BettingTips.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveSourceFromOutboundMessages : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.OutboundMessages", "Source");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OutboundMessages", "Source", c => c.String());
        }
    }
}
