namespace GpxViewer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakePointDoublesNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Points", "Latitude", c => c.Double());
            AlterColumn("dbo.Points", "Longitude", c => c.Double());
            AlterColumn("dbo.Points", "Elevation", c => c.Double());
            AlterColumn("dbo.Points", "PointCreatedAt", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Points", "PointCreatedAt", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Points", "Elevation", c => c.Double(nullable: false));
            AlterColumn("dbo.Points", "Longitude", c => c.Double(nullable: false));
            AlterColumn("dbo.Points", "Latitude", c => c.Double(nullable: false));
        }
    }
}
