namespace GpxViewer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCadenceToPoint : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Points", "Cadence", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Points", "Cadence");
        }
    }
}
