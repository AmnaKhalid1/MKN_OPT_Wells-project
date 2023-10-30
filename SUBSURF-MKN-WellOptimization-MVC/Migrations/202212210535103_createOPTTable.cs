namespace SUBSURF_MKN_WellOptimization_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createOPTTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Wellopts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RecordDate = c.DateTime(nullable: false),
                        UWI = c.String(),
                        WellName = c.String(),
                        Row = c.Int(nullable: false),
                        WellType = c.String(),
                        OPTIMIZATIONTYPES = c.Int(nullable: false),
                        Comments = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Wellopts");
        }
    }
}
