namespace Gyman.DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeTrainerModel : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Trainer", "Phone", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Trainer", "Phone", c => c.String(nullable: false));
        }
    }
}
