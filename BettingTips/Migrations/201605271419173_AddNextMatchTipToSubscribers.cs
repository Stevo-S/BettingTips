namespace BettingTips.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNextMatchTipToSubscribers : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Subscribers", "NextMatchTip", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Subscribers", "NextMatchTip");
        }
    }
}
