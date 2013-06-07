namespace GpxViewer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovePropertiesFromPoint : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Points", "Latitude", c => c.Double(nullable: false));
            AlterColumn("dbo.Points", "Longitude", c => c.Double(nullable: false));
            AlterColumn("dbo.Points", "Elevation", c => c.Double(nullable: false));
            DropColumn("dbo.Points", "Name");
            DropColumn("dbo.Points", "Description");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Points", "Description", c => c.String());
            AddColumn("dbo.Points", "Name", c => c.String());
            AlterColumn("dbo.Points", "Elevation", c => c.Single(nullable: false));
            AlterColumn("dbo.Points", "Longitude", c => c.Single(nullable: false));
            AlterColumn("dbo.Points", "Latitude", c => c.Single(nullable: false));
        }
    }
}
