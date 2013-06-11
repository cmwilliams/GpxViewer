namespace GpxViewer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDescriptionToTrack : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tracks", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tracks", "Description");
        }
    }
}
