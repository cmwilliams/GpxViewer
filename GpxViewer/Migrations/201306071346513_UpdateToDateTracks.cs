namespace GpxViewer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateToDateTracks : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Tracks", "FileSize", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Tracks", "FileSize", c => c.String());
        }
    }
}
