namespace GpxViewer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeDoubleToFloatForCoordinates : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Points", "Latitude", c => c.Single(nullable: false));
            AlterColumn("dbo.Points", "Longitude", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Points", "Longitude", c => c.Double(nullable: false));
            AlterColumn("dbo.Points", "Latitude", c => c.Double(nullable: false));
        }
    }
}
