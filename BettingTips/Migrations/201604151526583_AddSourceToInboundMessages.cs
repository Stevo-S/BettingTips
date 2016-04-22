namespace BettingTips.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSourceToInboundMessages : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InboundMessages", "source", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.InboundMessages", "source");
        }
    }
}
