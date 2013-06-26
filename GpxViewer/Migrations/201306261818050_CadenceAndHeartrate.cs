namespace GpxViewer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CadenceAndHeartrate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Activities", "MinimumHeartRate", c => c.Int());
            AddColumn("dbo.Activities", "MinimumCadence", c => c.Decimal(precision: 20, scale: 10));
            AddColumn("dbo.Activities", "MaximumCadence", c => c.Decimal(precision: 20, scale: 10));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Activities", "MaximumCadence");
            DropColumn("dbo.Activities", "MinimumCadence");
            DropColumn("dbo.Activities", "MinimumHeartRate");
        }
    }
}
