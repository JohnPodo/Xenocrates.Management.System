namespace ManagementSystemVersionTwo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SmallChangesToFirstStadium : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Projects", "StartDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Projects", "EndDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Projects", "EndDate");
            DropColumn("dbo.Projects", "StartDate");
        }
    }
}
