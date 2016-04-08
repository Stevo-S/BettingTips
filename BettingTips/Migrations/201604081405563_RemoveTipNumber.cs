namespace BettingTips.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveTipNumber : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Tips");
            AlterColumn("dbo.Tips", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Tips", "Id");
            DropColumn("dbo.Tips", "TipNumber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tips", "TipNumber", c => c.Int(nullable: false, identity: true));
            DropPrimaryKey("dbo.Tips");
            AlterColumn("dbo.Tips", "Id", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Tips", "Id");
        }
    }
}
