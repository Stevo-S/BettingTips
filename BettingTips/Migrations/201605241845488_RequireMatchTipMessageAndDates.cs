namespace BettingTips.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RequireMatchTipMessageAndDates : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.MatchSpecificTips", "Tip", c => c.String(nullable: false, maxLength: 900));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.MatchSpecificTips", "Tip", c => c.String(maxLength: 900));
        }
    }
}
