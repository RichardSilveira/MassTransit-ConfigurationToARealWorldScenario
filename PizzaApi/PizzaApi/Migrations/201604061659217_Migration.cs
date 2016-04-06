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
                        CustomerName = c.String(nullable: false, maxLength: 4000),
                        CustomerPhone = c.String(nullable: false, maxLength: 4000),
                        SenhaEspera = c.Int(),
                        EstimatedTime = c.Decimal(precision: 18, scale: 2),
                        Status = c.Int(nullable: false),
                        PizzaID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.OrderID);
            
            CreateTable(
                "dbo.Pizza",
                c => new
                    {
                        PizzaID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 4000),
                        Ingredients = c.String(nullable: false, maxLength: 4000),
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
