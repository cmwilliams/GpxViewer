namespace GpxViewer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakeTrackNameRequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Tracks", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Tracks", "Name", c => c.String());
        }
    }
}
