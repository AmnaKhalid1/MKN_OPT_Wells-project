namespace SUBSURF_MKN_WellOptimization_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modifyWells : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Wellopts", "UWI", c => c.String());
            AlterColumn("dbo.Wellopts", "Comments", c => c.String());
            DropColumn("dbo.Wellopts", "Row");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Wellopts", "Row", c => c.Int(nullable: false));
            AlterColumn("dbo.Wellopts", "Comments", c => c.String(nullable: false));
            AlterColumn("dbo.Wellopts", "UWI", c => c.String(nullable: false));
        }
    }
}
