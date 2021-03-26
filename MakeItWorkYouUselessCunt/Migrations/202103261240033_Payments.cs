namespace ManagementSystemVersionTwo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Payments : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PaymentDetails",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PaymentID = c.String(),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Date = c.DateTime(nullable: false),
                        Worker_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Workers", t => t.Worker_ID)
                .Index(t => t.Worker_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PaymentDetails", "Worker_ID", "dbo.Workers");
            DropIndex("dbo.PaymentDetails", new[] { "Worker_ID" });
            DropTable("dbo.PaymentDetails");
        }
    }
}
