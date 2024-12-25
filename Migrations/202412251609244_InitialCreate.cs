namespace online_is_bulma_platformu.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.JobApplications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        JobListingId = c.Int(nullable: false),
                        JobSeekerId = c.Int(nullable: false),
                        ApplicationDate = c.DateTime(nullable: false),
                        Message = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.JobListings", t => t.JobListingId, cascadeDelete: false)
                .ForeignKey("dbo.Users", t => t.JobSeekerId, cascadeDelete: true)
                .Index(t => t.JobListingId)
                .Index(t => t.JobSeekerId);
            
            CreateTable(
                "dbo.JobListings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        EmployerId = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        Deadline = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.EmployerId, cascadeDelete: true)
                .Index(t => t.EmployerId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Email = c.String(),
                        PasswordHash = c.String(),
                        Role = c.String(),
                        CreatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserMessages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SenderId = c.Int(nullable: false),
                        ReceiverId = c.Int(nullable: false),
                        JobListingId = c.Int(nullable: false),
                        Message = c.String(),
                        SentAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.JobListings", t => t.JobListingId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.ReceiverId, cascadeDelete: false)
                .ForeignKey("dbo.Users", t => t.SenderId, cascadeDelete: false)
                .Index(t => t.SenderId)
                .Index(t => t.ReceiverId)
                .Index(t => t.JobListingId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserMessages", "SenderId", "dbo.Users");
            DropForeignKey("dbo.UserMessages", "ReceiverId", "dbo.Users");
            DropForeignKey("dbo.UserMessages", "JobListingId", "dbo.JobListings");
            DropForeignKey("dbo.JobApplications", "JobSeekerId", "dbo.Users");
            DropForeignKey("dbo.JobApplications", "JobListingId", "dbo.JobListings");
            DropForeignKey("dbo.JobListings", "EmployerId", "dbo.Users");
            DropIndex("dbo.UserMessages", new[] { "JobListingId" });
            DropIndex("dbo.UserMessages", new[] { "ReceiverId" });
            DropIndex("dbo.UserMessages", new[] { "SenderId" });
            DropIndex("dbo.JobListings", new[] { "EmployerId" });
            DropIndex("dbo.JobApplications", new[] { "JobSeekerId" });
            DropIndex("dbo.JobApplications", new[] { "JobListingId" });
            DropTable("dbo.UserMessages");
            DropTable("dbo.Users");
            DropTable("dbo.JobListings");
            DropTable("dbo.JobApplications");
        }
    }
}
