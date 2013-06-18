namespace GpxViewer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangePrecisionForPoint : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Activities", "Distance", c => c.Decimal(precision: 20, scale: 10));
            AlterColumn("dbo.Activities", "AscendingDistance", c => c.Decimal(precision: 20, scale: 10));
            AlterColumn("dbo.Activities", "DescendingDistance", c => c.Decimal(precision: 20, scale: 10));
            AlterColumn("dbo.Activities", "FlatDistance", c => c.Decimal(precision: 20, scale: 10));
            AlterColumn("dbo.Activities", "ElevationGain", c => c.Decimal(precision: 20, scale: 10));
            AlterColumn("dbo.Activities", "ElevationLoss", c => c.Decimal(precision: 20, scale: 10));
            AlterColumn("dbo.Activities", "ElevationChange", c => c.Decimal(precision: 20, scale: 10));
            AlterColumn("dbo.Activities", "MaximumElevation", c => c.Decimal(precision: 20, scale: 10));
            AlterColumn("dbo.Activities", "MinimumElevation", c => c.Decimal(precision: 20, scale: 10));
            AlterColumn("dbo.Activities", "Duration", c => c.Decimal(precision: 20, scale: 10));
            AlterColumn("dbo.Activities", "ActiveDuration", c => c.Decimal(precision: 20, scale: 10));
            AlterColumn("dbo.Activities", "AscendingDuration", c => c.Decimal(precision: 20, scale: 10));
            AlterColumn("dbo.Activities", "DescendingDuration", c => c.Decimal(precision: 20, scale: 10));
            AlterColumn("dbo.Activities", "FlatDuration", c => c.Decimal(precision: 20, scale: 10));
            AlterColumn("dbo.Activities", "AveragePace", c => c.Decimal(precision: 20, scale: 10));
            AlterColumn("dbo.Activities", "AverageAscendingPace", c => c.Decimal(precision: 20, scale: 10));
            AlterColumn("dbo.Activities", "AverageDescendingPace", c => c.Decimal(precision: 20, scale: 10));
            AlterColumn("dbo.Activities", "AverageFlatPace", c => c.Decimal(precision: 20, scale: 10));
            AlterColumn("dbo.Activities", "AverageSpeed", c => c.Decimal(precision: 20, scale: 10));
            AlterColumn("dbo.Activities", "AverageAscendingSpeed", c => c.Decimal(precision: 20, scale: 10));
            AlterColumn("dbo.Activities", "AverageDescendingSpeed", c => c.Decimal(precision: 20, scale: 10));
            AlterColumn("dbo.Activities", "AverageFlatSpeed", c => c.Decimal(precision: 20, scale: 10));
            AlterColumn("dbo.Activities", "MaximumSpeed", c => c.Decimal(precision: 20, scale: 10));
            AlterColumn("dbo.Activities", "AverageHeartRate", c => c.Decimal(precision: 20, scale: 10));
            AlterColumn("dbo.Activities", "AverageCadence", c => c.Decimal(precision: 20, scale: 10));
            AlterColumn("dbo.Points", "Latitude", c => c.Decimal(precision: 20, scale: 10));
            AlterColumn("dbo.Points", "Longitude", c => c.Decimal(precision: 20, scale: 10));
            AlterColumn("dbo.Points", "Elevation", c => c.Decimal(precision: 20, scale: 10));
            AlterColumn("dbo.Points", "Distance", c => c.Decimal(precision: 20, scale: 10));
            AlterColumn("dbo.Points", "Duration", c => c.Decimal(precision: 20, scale: 10));
            AlterColumn("dbo.Points", "ActiveDuration", c => c.Decimal(precision: 20, scale: 10));
            AlterColumn("dbo.Points", "Pace", c => c.Decimal(precision: 20, scale: 10));
            AlterColumn("dbo.Points", "Speed", c => c.Decimal(precision: 20, scale: 10));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Points", "Speed", c => c.Decimal(precision: 20, scale: 14));
            AlterColumn("dbo.Points", "Pace", c => c.Decimal(precision: 20, scale: 14));
            AlterColumn("dbo.Points", "ActiveDuration", c => c.Decimal(precision: 20, scale: 14));
            AlterColumn("dbo.Points", "Duration", c => c.Decimal(precision: 20, scale: 14));
            AlterColumn("dbo.Points", "Distance", c => c.Decimal(precision: 20, scale: 14));
            AlterColumn("dbo.Points", "Elevation", c => c.Decimal(precision: 20, scale: 14));
            AlterColumn("dbo.Points", "Longitude", c => c.Decimal(precision: 20, scale: 14));
            AlterColumn("dbo.Points", "Latitude", c => c.Decimal(precision: 20, scale: 14));
            AlterColumn("dbo.Activities", "AverageCadence", c => c.Decimal(precision: 20, scale: 14));
            AlterColumn("dbo.Activities", "AverageHeartRate", c => c.Decimal(precision: 20, scale: 14));
            AlterColumn("dbo.Activities", "MaximumSpeed", c => c.Decimal(precision: 20, scale: 14));
            AlterColumn("dbo.Activities", "AverageFlatSpeed", c => c.Decimal(precision: 20, scale: 14));
            AlterColumn("dbo.Activities", "AverageDescendingSpeed", c => c.Decimal(precision: 20, scale: 14));
            AlterColumn("dbo.Activities", "AverageAscendingSpeed", c => c.Decimal(precision: 20, scale: 14));
            AlterColumn("dbo.Activities", "AverageSpeed", c => c.Decimal(precision: 20, scale: 14));
            AlterColumn("dbo.Activities", "AverageFlatPace", c => c.Decimal(precision: 20, scale: 14));
            AlterColumn("dbo.Activities", "AverageDescendingPace", c => c.Decimal(precision: 20, scale: 14));
            AlterColumn("dbo.Activities", "AverageAscendingPace", c => c.Decimal(precision: 20, scale: 14));
            AlterColumn("dbo.Activities", "AveragePace", c => c.Decimal(precision: 20, scale: 14));
            AlterColumn("dbo.Activities", "FlatDuration", c => c.Decimal(precision: 20, scale: 14));
            AlterColumn("dbo.Activities", "DescendingDuration", c => c.Decimal(precision: 20, scale: 14));
            AlterColumn("dbo.Activities", "AscendingDuration", c => c.Decimal(precision: 20, scale: 14));
            AlterColumn("dbo.Activities", "ActiveDuration", c => c.Decimal(precision: 20, scale: 14));
            AlterColumn("dbo.Activities", "Duration", c => c.Decimal(precision: 20, scale: 14));
            AlterColumn("dbo.Activities", "MinimumElevation", c => c.Decimal(precision: 20, scale: 14));
            AlterColumn("dbo.Activities", "MaximumElevation", c => c.Decimal(precision: 20, scale: 14));
            AlterColumn("dbo.Activities", "ElevationChange", c => c.Decimal(precision: 20, scale: 14));
            AlterColumn("dbo.Activities", "ElevationLoss", c => c.Decimal(precision: 20, scale: 14));
            AlterColumn("dbo.Activities", "ElevationGain", c => c.Decimal(precision: 20, scale: 14));
            AlterColumn("dbo.Activities", "FlatDistance", c => c.Decimal(precision: 20, scale: 14));
            AlterColumn("dbo.Activities", "DescendingDistance", c => c.Decimal(precision: 20, scale: 14));
            AlterColumn("dbo.Activities", "AscendingDistance", c => c.Decimal(precision: 20, scale: 14));
            AlterColumn("dbo.Activities", "Distance", c => c.Decimal(precision: 20, scale: 14));
        }
    }
}
