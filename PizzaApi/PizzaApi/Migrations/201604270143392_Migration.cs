namespace PizzaApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Order",
                c => new
                    {
                        OrderID = c.Int(nullable: false, identity: true),
                        CustomerName = c.String(nullable: false),
                        CustomerPhone = c.String(nullable: false),
                        EstimatedTime = c.Int(),
                        Status = c.Int(nullable: false),
                        RejectedReasonPhrase = c.String(),
                        PizzaID = c.Int(nullable: false),
                        CorrelationId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.OrderID);
            
            CreateTable(
                "dbo.Pizza",
                c => new
                    {
                        PizzaID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Ingredients = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.PizzaID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Pizza");
            DropTable("dbo.Order");
        }
    }
}
