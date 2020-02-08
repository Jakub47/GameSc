namespace Thesis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addisHappytomodels : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CommentEvent", "IsHappy", c => c.Boolean(nullable: false));
            AddColumn("dbo.CommentPost", "IsHappy", c => c.Boolean(nullable: false));
            AddColumn("dbo.Post", "IsHappy", c => c.Boolean(nullable: false));
            AddColumn("dbo.Content", "IsHappy", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Content", "IsHappy");
            DropColumn("dbo.Post", "IsHappy");
            DropColumn("dbo.CommentPost", "IsHappy");
            DropColumn("dbo.CommentEvent", "IsHappy");
        }
    }
}
