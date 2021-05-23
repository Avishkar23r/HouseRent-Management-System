namespace HouseRentManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialDB : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Customer_Data",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        MobileNumber = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Password = c.String(nullable: false, maxLength: 20),
                        ProfilePic = c.String(nullable: false),
                        State = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Seller_Data",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        MobileNumber = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Password = c.String(nullable: false, maxLength: 20),
                        ProfilePic = c.String(nullable: false),
                        State = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Seller_Data");
            DropTable("dbo.Customer_Data");
        }
    }
}
