namespace Gyman.DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Member", "SubscriptionId", "dbo.Subscription");
            DropIndex("dbo.Member", new[] { "SubscriptionId" });
            AlterColumn("dbo.Member", "SubscriptionId", c => c.Int());
            CreateIndex("dbo.Member", "SubscriptionId");
            AddForeignKey("dbo.Member", "SubscriptionId", "dbo.Subscription", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Member", "SubscriptionId", "dbo.Subscription");
            DropIndex("dbo.Member", new[] { "SubscriptionId" });
            AlterColumn("dbo.Member", "SubscriptionId", c => c.Int(nullable: false));
            CreateIndex("dbo.Member", "SubscriptionId");
            AddForeignKey("dbo.Member", "SubscriptionId", "dbo.Subscription", "Id", cascadeDelete: true);
        }
    }
}
