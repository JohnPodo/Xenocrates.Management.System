namespace ManagementSystemVersionTwo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Message : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Text = c.String(),
                        Date = c.DateTime(nullable: false),
                        Department_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Departments", t => t.Department_ID)
                .Index(t => t.Department_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Messages", "Department_ID", "dbo.Departments");
            DropIndex("dbo.Messages", new[] { "Department_ID" });
            DropTable("dbo.Messages");
        }
    }
}
