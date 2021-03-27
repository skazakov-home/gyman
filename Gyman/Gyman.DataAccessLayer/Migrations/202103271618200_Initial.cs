namespace Gyman.DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Member",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Surname = c.String(maxLength: 50),
                        Email = c.String(),
                        Phone = c.String(nullable: false),
                        Age = c.Int(nullable: false),
                        Weight = c.Double(nullable: false),
                        Height = c.Double(nullable: false),
                        SubscriptionId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Subscription", t => t.SubscriptionId)
                .Index(t => t.SubscriptionId);
            
            CreateTable(
                "dbo.Subscription",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Cost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        NumberOfDays = c.Int(nullable: false),
                        SubscriptionType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Trainer",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Surname = c.String(maxLength: 50),
                        Phone = c.String(),
                        IsBusy = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Training",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Start = c.DateTime(nullable: false),
                        End = c.DateTime(nullable: false),
                        TrainerId = c.Int(nullable: false),
                        MemberId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Member", t => t.MemberId, cascadeDelete: true)
                .ForeignKey("dbo.Trainer", t => t.TrainerId, cascadeDelete: true)
                .Index(t => t.TrainerId)
                .Index(t => t.MemberId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Training", "TrainerId", "dbo.Trainer");
            DropForeignKey("dbo.Training", "MemberId", "dbo.Member");
            DropForeignKey("dbo.Member", "SubscriptionId", "dbo.Subscription");
            DropIndex("dbo.Training", new[] { "MemberId" });
            DropIndex("dbo.Training", new[] { "TrainerId" });
            DropIndex("dbo.Member", new[] { "SubscriptionId" });
            DropTable("dbo.Training");
            DropTable("dbo.Trainer");
            DropTable("dbo.Subscription");
            DropTable("dbo.Member");
        }
    }
}
