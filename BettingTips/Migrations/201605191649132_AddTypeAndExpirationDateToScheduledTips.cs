namespace BettingTips.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTypeAndExpirationDateToScheduledTips : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ScheduledTips", "ExpirationDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.ScheduledTips", "Type", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ScheduledTips", "Type");
            DropColumn("dbo.ScheduledTips", "ExpirationDate");
        }
    }
}
