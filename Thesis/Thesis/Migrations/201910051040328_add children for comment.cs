namespace Thesis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addchildrenforcomment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CommentEvent", "ParentCommentEventId", c => c.Int());
            AddColumn("dbo.CommentPost", "ParentCommentPostId", c => c.Int());
            CreateIndex("dbo.CommentEvent", "ParentCommentEventId");
            CreateIndex("dbo.CommentPost", "ParentCommentPostId");
            AddForeignKey("dbo.CommentEvent", "ParentCommentEventId", "dbo.CommentEvent", "CommentEventId");
            AddForeignKey("dbo.CommentPost", "ParentCommentPostId", "dbo.CommentPost", "CommentPostId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CommentPost", "ParentCommentPostId", "dbo.CommentPost");
            DropForeignKey("dbo.CommentEvent", "ParentCommentEventId", "dbo.CommentEvent");
            DropIndex("dbo.CommentPost", new[] { "ParentCommentPostId" });
            DropIndex("dbo.CommentEvent", new[] { "ParentCommentEventId" });
            DropColumn("dbo.CommentPost", "ParentCommentPostId");
            DropColumn("dbo.CommentEvent", "ParentCommentEventId");
        }
    }
}
