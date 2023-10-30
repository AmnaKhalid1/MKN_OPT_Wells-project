namespace SUBSURF_MKN_WellOptimization_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class logActionAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ActionLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LogDate = c.DateTime(nullable: false),
                        RecordId = c.Int(nullable: false),
                        Username = c.String(nullable: false),
                        actionType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ActionLogs");
        }
    }
}
