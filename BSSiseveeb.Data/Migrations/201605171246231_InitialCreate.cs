namespace BSSiseveeb.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Birthdate = c.DateTime(),
                        ContractStart = c.DateTime(),
                        ContractEnd = c.DateTime(),
                        PhoneNumber = c.String(),
                        VacationDays = c.Int(nullable: false),
                        Email = c.String(),
                        VacationMessages = c.Boolean(nullable: false),
                        RequestMessages = c.Boolean(nullable: false),
                        MonthlyBirthdayMessages = c.Boolean(nullable: false),
                        DailyBirthdayMessages = c.Boolean(nullable: false),
                        RoleId = c.Int(nullable: false),
                        IsInitialized = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Rights = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Requests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Req = c.String(),
                        Description = c.String(),
                        Status = c.Int(nullable: false),
                        EmployeeId = c.String(maxLength: 128),
                        TimeStamp = c.DateTime(nullable: false),
                        Cleared = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.EmployeeId)
                .Index(t => t.EmployeeId);
            
            CreateTable(
                "dbo.UserTokenCaches",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        webUserUniqueId = c.String(),
                        cacheBits = c.Binary(),
                        LastWrite = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Vacations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        Days = c.Int(nullable: false),
                        EmployeeId = c.String(maxLength: 128),
                        Status = c.Int(nullable: false),
                        Comments = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.EmployeeId)
                .Index(t => t.EmployeeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Vacations", "EmployeeId", "dbo.Employees");
            DropForeignKey("dbo.Requests", "EmployeeId", "dbo.Employees");
            DropForeignKey("dbo.Employees", "RoleId", "dbo.Roles");
            DropIndex("dbo.Vacations", new[] { "EmployeeId" });
            DropIndex("dbo.Requests", new[] { "EmployeeId" });
            DropIndex("dbo.Employees", new[] { "RoleId" });
            DropTable("dbo.Vacations");
            DropTable("dbo.UserTokenCaches");
            DropTable("dbo.Requests");
            DropTable("dbo.Roles");
            DropTable("dbo.Employees");
        }
    }
}
