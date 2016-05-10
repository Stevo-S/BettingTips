namespace BettingTips.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDeliveries : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Deliveries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Destination = c.String(maxLength: 20),
                        Message = c.String(maxLength: 1000),
                        ServiceId = c.String(maxLength: 50),
                        Correlator = c.Int(nullable: false),
                        TraceUniqueId = c.String(maxLength: 100),
                        TimeStamp = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Deliveries");
        }
    }
}
