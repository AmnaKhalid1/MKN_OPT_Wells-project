namespace SUBSURF_MKN_WellOptimization_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveWllTypeOPTTable : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Wellopts", "WellType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Wellopts", "WellType", c => c.String(nullable: false));
        }
    }
}
