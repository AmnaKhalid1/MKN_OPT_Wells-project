namespace SUBSURF_MKN_WellOptimization_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditOPTTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Wellopts", "PumpType", c => c.String(nullable: false));
            AlterColumn("dbo.Wellopts", "UWI", c => c.String(nullable: false));
            AlterColumn("dbo.Wellopts", "WellName", c => c.String(nullable: false));
            AlterColumn("dbo.Wellopts", "WellType", c => c.String(nullable: false));
            AlterColumn("dbo.Wellopts", "Comments", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Wellopts", "Comments", c => c.String());
            AlterColumn("dbo.Wellopts", "WellType", c => c.String());
            AlterColumn("dbo.Wellopts", "WellName", c => c.String());
            AlterColumn("dbo.Wellopts", "UWI", c => c.String());
            DropColumn("dbo.Wellopts", "PumpType");
        }
    }
}
