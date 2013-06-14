namespace GpxViewer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ActivityDateToActivity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Activities", "ActivityDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Activities", "ActivityDate");
        }
    }
}
