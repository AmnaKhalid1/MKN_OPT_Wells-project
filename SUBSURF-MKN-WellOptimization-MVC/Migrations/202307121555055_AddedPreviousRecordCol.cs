namespace SUBSURF_MKN_WellOptimization_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedPreviousRecordCol : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ActionLogs", "PreviousRecord", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ActionLogs", "PreviousRecord");
        }
    }
}
