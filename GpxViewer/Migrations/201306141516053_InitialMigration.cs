namespace GpxViewer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Activities",
                c => new
                    {
                        ActivityId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Distance = c.Decimal(precision: 20, scale: 10),
                        AscendingDistance = c.Decimal(precision: 20, scale: 10),
                        DescendingDistance = c.Decimal(precision: 20, scale: 10),
                        FlatDistance = c.Decimal(precision: 20, scale: 10),
                        ElevationGain = c.Decimal(precision: 20, scale: 10),
                        ElevationLoss = c.Decimal(precision: 20, scale: 10),
                        ElevationChange = c.Decimal(precision: 20, scale: 10),
                        MaximumElevation = c.Decimal(precision: 20, scale: 10),
                        MinimumElevation = c.Decimal(precision: 20, scale: 10),
                        Duration = c.Decimal(precision: 20, scale: 10),
                        ActiveDuration = c.Decimal(precision: 20, scale: 10),
                        AscendingDuration = c.Decimal(precision: 20, scale: 10),
                        DescendingDuration = c.Decimal(precision: 20, scale: 10),
                        FlatDuration = c.Decimal(precision: 20, scale: 10),
                        AveragePace = c.Decimal(precision: 20, scale: 10),
                        AverageAscendingPace = c.Decimal(precision: 20, scale: 10),
                        AverageDescendingPace = c.Decimal(precision: 20, scale: 10),
                        AverageFlatPace = c.Decimal(precision: 20, scale: 10),
                        AverageSpeed = c.Decimal(precision: 20, scale: 10),
                        AverageAscendingSpeed = c.Decimal(precision: 20, scale: 10),
                        AverageDescendingSpeed = c.Decimal(precision: 20, scale: 10),
                        AverageFlatSpeed = c.Decimal(precision: 20, scale: 10),
                        MaximumSpeed = c.Decimal(precision: 20, scale: 10),
                        MaximumHeartRate = c.Int(),
                        AverageHeartRate = c.Decimal(precision: 20, scale: 10),
                        AverageCadence = c.Decimal(precision: 20, scale: 10),
                    })
                .PrimaryKey(t => t.ActivityId);
            
            CreateTable(
                "dbo.Points",
                c => new
                    {
                        PointId = c.Int(nullable: false, identity: true),
                        Time = c.DateTime(),
                        Latitude = c.Decimal(precision: 20, scale: 10),
                        Longitude = c.Decimal(precision: 20, scale: 10),
                        Elevation = c.Decimal(precision: 20, scale: 10),
                        Distance = c.Decimal(precision: 20, scale: 10),
                        Duration = c.Decimal(precision: 20, scale: 10),
                        ActiveDuration = c.Decimal(precision: 20, scale: 10),
                        Pace = c.Decimal(precision: 20, scale: 10),
                        Speed = c.Decimal(precision: 20, scale: 10),
                        HeartRate = c.Int(),
                        Cadence = c.Int(),
                        ActivityId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PointId)
                .ForeignKey("dbo.Activities", t => t.ActivityId, cascadeDelete: true)
                .Index(t => t.ActivityId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Points", new[] { "ActivityId" });
            DropForeignKey("dbo.Points", "ActivityId", "dbo.Activities");
            DropTable("dbo.Points");
            DropTable("dbo.Activities");
        }
    }
}
