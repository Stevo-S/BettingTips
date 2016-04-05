namespace BettingTips.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AutoIncrementTipNumber : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Tips");
            AlterColumn("dbo.Tips", "Id", c => c.Int(nullable: false));
            AlterColumn("dbo.Tips", "TipNumber", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Tips", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.Tips");
            AlterColumn("dbo.Tips", "TipNumber", c => c.Int(nullable: false));
            AlterColumn("dbo.Tips", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Tips", "Id");
        }
    }
}
