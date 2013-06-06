namespace GpxViewer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FileAttributesToTracks : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tracks", "FileName", c => c.String());
            AddColumn("dbo.Tracks", "FileContentType", c => c.String());
            AddColumn("dbo.Tracks", "FileSize", c => c.String());
            AddColumn("dbo.Tracks", "FileUpdatedAt", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tracks", "FileUpdatedAt");
            DropColumn("dbo.Tracks", "FileSize");
            DropColumn("dbo.Tracks", "FileContentType");
            DropColumn("dbo.Tracks", "FileName");
        }
    }
}
