namespace ManagementSystemVersionTwo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProjectAttachmentAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Projects", "Attachments", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Projects", "Attachments");
        }
    }
}
