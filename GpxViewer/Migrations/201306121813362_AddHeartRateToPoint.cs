namespace GpxViewer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddHeartRateToPoint : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Points", "HeartRate", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Points", "HeartRate");
        }
    }
}
