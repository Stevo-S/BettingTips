namespace BettingTips.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIndexesToScheduledTips : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ScheduledTips", "Destination", c => c.String(maxLength: 20));
            CreateIndex("dbo.ScheduledTips", "DateScheduled");
            CreateIndex("dbo.ScheduledTips", "ExpirationDate");
            CreateIndex("dbo.ScheduledTips", "Destination");
        }
        
        public override void Down()
        {
            DropIndex("dbo.ScheduledTips", new[] { "Destination" });
            DropIndex("dbo.ScheduledTips", new[] { "ExpirationDate" });
            DropIndex("dbo.ScheduledTips", new[] { "DateScheduled" });
            AlterColumn("dbo.ScheduledTips", "Destination", c => c.String());
        }
    }
}
