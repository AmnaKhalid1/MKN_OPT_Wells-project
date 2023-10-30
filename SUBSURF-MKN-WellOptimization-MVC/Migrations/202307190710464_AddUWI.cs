namespace SUBSURF_MKN_WellOptimization_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUWI : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ActionLogs", "UWI", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ActionLogs", "UWI");
        }
    }
}
