namespace ManagementSystemVersionTwo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Calendar : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WorkingDays",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Start = c.String(),
                        End = c.String(),
                        Title = c.String(),
                        Display = c.String(),
                        BackgroundColor = c.String(),
                        Worker_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Workers", t => t.Worker_ID)
                .Index(t => t.Worker_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WorkingDays", "Worker_ID", "dbo.Workers");
            DropIndex("dbo.WorkingDays", new[] { "Worker_ID" });
            DropTable("dbo.WorkingDays");
        }
    }
}
