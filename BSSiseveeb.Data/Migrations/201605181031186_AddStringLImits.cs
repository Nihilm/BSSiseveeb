namespace BSSiseveeb.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStringLImits : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Employees", "Name", c => c.String(maxLength: 128));
            AlterColumn("dbo.Employees", "PhoneNumber", c => c.String(maxLength: 32));
            AlterColumn("dbo.Roles", "Name", c => c.String(maxLength: 32));
            AlterColumn("dbo.Requests", "Req", c => c.String(maxLength: 128));
            AlterColumn("dbo.Requests", "Description", c => c.String(maxLength: 512));
            AlterColumn("dbo.Vacations", "Comments", c => c.String(maxLength: 512));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Vacations", "Comments", c => c.String());
            AlterColumn("dbo.Requests", "Description", c => c.String());
            AlterColumn("dbo.Requests", "Req", c => c.String());
            AlterColumn("dbo.Roles", "Name", c => c.String());
            AlterColumn("dbo.Employees", "PhoneNumber", c => c.String());
            AlterColumn("dbo.Employees", "Name", c => c.String());
        }
    }
}
