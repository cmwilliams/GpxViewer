namespace GpxViewer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tracks",
                c => new
                    {
                        TrackId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.TrackId);
            
            CreateTable(
                "dbo.TrackSegments",
                c => new
                    {
                        TrackSegmentId = c.Int(nullable: false, identity: true),
                        TrackId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TrackSegmentId)
                .ForeignKey("dbo.Tracks", t => t.TrackId, cascadeDelete: true)
                .Index(t => t.TrackId);
            
            CreateTable(
                "dbo.Points",
                c => new
                    {
                        PointId = c.Int(nullable: false, identity: true),
                        TrackSegmentId = c.Int(nullable: false),
                        Name = c.String(),
                        Latitude = c.Single(nullable: false),
                        Longitude = c.Single(nullable: false),
                        Elevation = c.Single(nullable: false),
                        Description = c.String(),
                        PointCreatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.PointId)
                .ForeignKey("dbo.TrackSegments", t => t.TrackSegmentId, cascadeDelete: true)
                .Index(t => t.TrackSegmentId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Points", new[] { "TrackSegmentId" });
            DropIndex("dbo.TrackSegments", new[] { "TrackId" });
            DropForeignKey("dbo.Points", "TrackSegmentId", "dbo.TrackSegments");
            DropForeignKey("dbo.TrackSegments", "TrackId", "dbo.Tracks");
            DropTable("dbo.Points");
            DropTable("dbo.TrackSegments");
            DropTable("dbo.Tracks");
        }
    }
}
