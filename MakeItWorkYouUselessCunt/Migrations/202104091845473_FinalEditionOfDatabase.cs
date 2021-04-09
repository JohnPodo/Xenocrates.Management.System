namespace ManagementSystemVersionTwo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FinalEditionOfDatabase : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.PaymentDetails", "PaymentID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PaymentDetails", "PaymentID", c => c.String());
        }
    }
}
