namespace HouseRentManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BookingDetails : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Booking_Details",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PropertyId = c.Int(nullable: false),
                        CustomerId = c.Int(nullable: false),
                        Booking_Status = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropColumn("dbo.Property_Details", "Requested_By");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Property_Details", "Requested_By", c => c.String());
            DropTable("dbo.Booking_Details");
        }
    }
}
