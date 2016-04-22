namespace BettingTips.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLinkIdToMessages : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InboundMessages", "LinkID", c => c.String());
            AddColumn("dbo.OutboundMessages", "LinkID", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.OutboundMessages", "LinkID");
            DropColumn("dbo.InboundMessages", "LinkID");
        }
    }
}
