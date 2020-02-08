namespace Thesis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CategoryEvent",
                c => new
                    {
                        CategoryEventId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Description = c.String(nullable: false, maxLength: 150),
                        Picture = c.String(nullable: false),
                        Icon = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.CategoryEventId);
            
            CreateTable(
                "dbo.GamingEvent",
                c => new
                    {
                        GamingEventId = c.Int(nullable: false, identity: true),
                        CategoryEventId = c.Int(nullable: false),
                        UserId = c.String(),
                        Title = c.String(nullable: false, maxLength: 75),
                        Content = c.String(nullable: false, maxLength: 1000),
                        DateOfEvent = c.DateTime(nullable: false),
                        MaxNumberOfPeople = c.Int(nullable: false),
                        CurrentNumberOfPeople = c.Int(nullable: false),
                        MainPicture = c.String(),
                        Publisher_Id = c.String(maxLength: 128),
                        Group_GroupId = c.Int(),
                    })
                .PrimaryKey(t => t.GamingEventId)
                .ForeignKey("dbo.CategoryEvent", t => t.CategoryEventId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.Publisher_Id)
                .ForeignKey("dbo.Group", t => t.Group_GroupId)
                .Index(t => t.CategoryEventId)
                .Index(t => t.Publisher_Id)
                .Index(t => t.Group_GroupId);
            
            CreateTable(
                "dbo.CommentEvent",
                c => new
                    {
                        CommentEventId = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        Body = c.String(nullable: false, maxLength: 100),
                        DateOfInsert = c.DateTime(nullable: false),
                        Likes = c.Int(nullable: false),
                        UnLikes = c.Int(nullable: false),
                        Hidden = c.Boolean(nullable: false),
                        GamingEventId = c.Int(),
                    })
                .PrimaryKey(t => t.CommentEventId)
                .ForeignKey("dbo.GamingEvent", t => t.GamingEventId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.GamingEventId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        UserInformation_FirstName = c.String(),
                        UserInformation_LastName = c.String(),
                        UserInformation_Adress = c.String(),
                        UserInformation_Town = c.String(),
                        UserInformation_PostCode = c.String(),
                        UserInformation_PhoneNumber = c.String(),
                        UserInformation_Email = c.String(),
                        MainPicture = c.String(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.CommentPost",
                c => new
                    {
                        CommentPostId = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        Body = c.String(nullable: false, maxLength: 100),
                        DateOfInsert = c.DateTime(nullable: false),
                        Likes = c.Int(nullable: false),
                        UnLikes = c.Int(nullable: false),
                        Hidden = c.Boolean(nullable: false),
                        PostId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CommentPostId)
                .ForeignKey("dbo.Post", t => t.PostId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.PostId);
            
            CreateTable(
                "dbo.Post",
                c => new
                    {
                        PostId = c.Int(nullable: false, identity: true),
                        CategoryPostId = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                        Title = c.String(nullable: false, maxLength: 75),
                        Content = c.String(nullable: false, maxLength: 750),
                        DateOfInsert = c.DateTime(nullable: false),
                        Likes = c.Int(nullable: false),
                        UnLikes = c.Int(nullable: false),
                        MainPicture = c.String(),
                    })
                .PrimaryKey(t => t.PostId)
                .ForeignKey("dbo.CategoryPost", t => t.CategoryPostId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.CategoryPostId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.CategoryPost",
                c => new
                    {
                        CategoryPostId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Description = c.String(nullable: false, maxLength: 150),
                        Picture = c.String(nullable: false),
                        Icon = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.CategoryPostId);
            
            CreateTable(
                "dbo.ImagePost",
                c => new
                    {
                        ImagePostId = c.Int(nullable: false, identity: true),
                        FilePath = c.String(),
                        PostId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ImagePostId)
                .ForeignKey("dbo.Post", t => t.PostId, cascadeDelete: true)
                .Index(t => t.PostId);
            
            CreateTable(
                "dbo.Conversation",
                c => new
                    {
                        ConversationId = c.Int(nullable: false, identity: true),
                        SenderId = c.String(nullable: false, maxLength: 128),
                        ReceiverId = c.String(nullable: false, maxLength: 128),
                        SenderReceived = c.Boolean(nullable: false),
                        ReceiverReceived = c.Boolean(nullable: false),
                        LastDateTimeSend = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ConversationId)
                .ForeignKey("dbo.AspNetUsers", t => t.ReceiverId, cascadeDelete: false)
                .ForeignKey("dbo.AspNetUsers", t => t.SenderId, cascadeDelete: false)
                .Index(t => t.SenderId)
                .Index(t => t.ReceiverId);
            
            CreateTable(
                "dbo.Content",
                c => new
                    {
                        ContentId = c.Int(nullable: false, identity: true),
                        ConversationId = c.Int(nullable: false),
                        UserId = c.String(),
                        MessageContent = c.String(),
                        SendDate = c.DateTime(nullable: false),
                        UserSender_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ContentId)
                .ForeignKey("dbo.Conversation", t => t.ConversationId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserSender_Id)
                .Index(t => t.ConversationId)
                .Index(t => t.UserSender_Id);
            
            CreateTable(
                "dbo.Game",
                c => new
                    {
                        GameId = c.Int(nullable: false, identity: true),
                        CategoryGameId = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                        Title = c.String(nullable: false, maxLength: 150),
                        Publisher = c.String(nullable: false, maxLength: 150),
                        Description = c.String(nullable: false, maxLength: 1000),
                        ShortDescription = c.String(nullable: false, maxLength: 150),
                        Hidden = c.Boolean(nullable: false),
                        MainPicture = c.String(),
                        GamesForExchange = c.String(),
                    })
                .PrimaryKey(t => t.GameId)
                .ForeignKey("dbo.CategoryGame", t => t.CategoryGameId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.CategoryGameId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.CategoryGame",
                c => new
                    {
                        CategoryGameId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Description = c.String(nullable: false, maxLength: 150),
                        Picture = c.String(nullable: false),
                        Icon = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.CategoryGameId);
            
            CreateTable(
                "dbo.ImageGame",
                c => new
                    {
                        ImageGameId = c.Int(nullable: false, identity: true),
                        FilePath = c.String(),
                        GameId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ImageGameId)
                .ForeignKey("dbo.Game", t => t.GameId, cascadeDelete: true)
                .Index(t => t.GameId);
            
            CreateTable(
                "dbo.Question",
                c => new
                    {
                        QuestionId = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        GameId = c.Int(nullable: false),
                        QuestionContent = c.String(nullable: false, maxLength: 100),
                        ReplyContent = c.String(),
                        isReadedBySender = c.Boolean(nullable: false),
                        isReadedByReceiver = c.Boolean(nullable: false),
                        QuestionDate = c.DateTime(nullable: false),
                        ReplyDate = c.DateTime(nullable: false),
                        Sender_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.QuestionId)
                .ForeignKey("dbo.Game", t => t.GameId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.Sender_Id)
                .Index(t => t.GameId)
                .Index(t => t.Sender_Id);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.ImageEvent",
                c => new
                    {
                        ImageEventId = c.Int(nullable: false, identity: true),
                        FilePath = c.String(),
                        GamingEventId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ImageEventId)
                .ForeignKey("dbo.GamingEvent", t => t.GamingEventId, cascadeDelete: true)
                .Index(t => t.GamingEventId);
            
            CreateTable(
                "dbo.Group",
                c => new
                    {
                        GroupId = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 75),
                        Information = c.String(nullable: false, maxLength: 300),
                        Image = c.String(),
                    })
                .PrimaryKey(t => t.GroupId);
            
            CreateTable(
                "dbo.ImageGroup",
                c => new
                    {
                        ImageGroupId = c.Int(nullable: false, identity: true),
                        FilePath = c.String(),
                        GroupId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ImageGroupId)
                .ForeignKey("dbo.Group", t => t.GroupId, cascadeDelete: true)
                .Index(t => t.GroupId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.ApplicationUserCommentEvent",
                c => new
                    {
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                        CommentEvent_CommentEventId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ApplicationUser_Id, t.CommentEvent_CommentEventId })
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id, cascadeDelete: true)
                .ForeignKey("dbo.CommentEvent", t => t.CommentEvent_CommentEventId, cascadeDelete: true)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.CommentEvent_CommentEventId);
            
            CreateTable(
                "dbo.ApplicationUserCommentPost",
                c => new
                    {
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                        CommentPost_CommentPostId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ApplicationUser_Id, t.CommentPost_CommentPostId })
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id, cascadeDelete: true)
                .ForeignKey("dbo.CommentPost", t => t.CommentPost_CommentPostId, cascadeDelete: true)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.CommentPost_CommentPostId);
            
            CreateTable(
                "dbo.ApplicationUserPost",
                c => new
                    {
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                        Post_PostId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ApplicationUser_Id, t.Post_PostId })
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id, cascadeDelete: true)
                .ForeignKey("dbo.Post", t => t.Post_PostId, cascadeDelete: true)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.Post_PostId);
            
            CreateTable(
                "dbo.ApplicationUserCommentEvent1",
                c => new
                    {
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                        CommentEvent_CommentEventId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ApplicationUser_Id, t.CommentEvent_CommentEventId })
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id, cascadeDelete: true)
                .ForeignKey("dbo.CommentEvent", t => t.CommentEvent_CommentEventId, cascadeDelete: true)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.CommentEvent_CommentEventId);
            
            CreateTable(
                "dbo.ApplicationUserCommentPost1",
                c => new
                    {
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                        CommentPost_CommentPostId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ApplicationUser_Id, t.CommentPost_CommentPostId })
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id, cascadeDelete: true)
                .ForeignKey("dbo.CommentPost", t => t.CommentPost_CommentPostId, cascadeDelete: true)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.CommentPost_CommentPostId);
            
            CreateTable(
                "dbo.ApplicationUserPost1",
                c => new
                    {
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                        Post_PostId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ApplicationUser_Id, t.Post_PostId })
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id, cascadeDelete: true)
                .ForeignKey("dbo.Post", t => t.Post_PostId, cascadeDelete: true)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.Post_PostId);
            
            CreateTable(
                "dbo.GamingEventApplicationUser",
                c => new
                    {
                        GamingEvent_GamingEventId = c.Int(nullable: false),
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.GamingEvent_GamingEventId, t.ApplicationUser_Id })
                .ForeignKey("dbo.GamingEvent", t => t.GamingEvent_GamingEventId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id, cascadeDelete: true)
                .Index(t => t.GamingEvent_GamingEventId)
                .Index(t => t.ApplicationUser_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.ImageGroup", "GroupId", "dbo.Group");
            DropForeignKey("dbo.GamingEvent", "Group_GroupId", "dbo.Group");
            DropForeignKey("dbo.GamingEventApplicationUser", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.GamingEventApplicationUser", "GamingEvent_GamingEventId", "dbo.GamingEvent");
            DropForeignKey("dbo.ImageEvent", "GamingEventId", "dbo.GamingEvent");
            DropForeignKey("dbo.GamingEvent", "Publisher_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ApplicationUserPost1", "Post_PostId", "dbo.Post");
            DropForeignKey("dbo.ApplicationUserPost1", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ApplicationUserCommentPost1", "CommentPost_CommentPostId", "dbo.CommentPost");
            DropForeignKey("dbo.ApplicationUserCommentPost1", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ApplicationUserCommentEvent1", "CommentEvent_CommentEventId", "dbo.CommentEvent");
            DropForeignKey("dbo.ApplicationUserCommentEvent1", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ApplicationUserPost", "Post_PostId", "dbo.Post");
            DropForeignKey("dbo.ApplicationUserPost", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ApplicationUserCommentPost", "CommentPost_CommentPostId", "dbo.CommentPost");
            DropForeignKey("dbo.ApplicationUserCommentPost", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ApplicationUserCommentEvent", "CommentEvent_CommentEventId", "dbo.CommentEvent");
            DropForeignKey("dbo.ApplicationUserCommentEvent", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Game", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Question", "Sender_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Question", "GameId", "dbo.Game");
            DropForeignKey("dbo.ImageGame", "GameId", "dbo.Game");
            DropForeignKey("dbo.Game", "CategoryGameId", "dbo.CategoryGame");
            DropForeignKey("dbo.Conversation", "SenderId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Conversation", "ReceiverId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Content", "UserSender_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Content", "ConversationId", "dbo.Conversation");
            DropForeignKey("dbo.CommentPost", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Post", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ImagePost", "PostId", "dbo.Post");
            DropForeignKey("dbo.CommentPost", "PostId", "dbo.Post");
            DropForeignKey("dbo.Post", "CategoryPostId", "dbo.CategoryPost");
            DropForeignKey("dbo.CommentEvent", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.CommentEvent", "GamingEventId", "dbo.GamingEvent");
            DropForeignKey("dbo.GamingEvent", "CategoryEventId", "dbo.CategoryEvent");
            DropIndex("dbo.GamingEventApplicationUser", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.GamingEventApplicationUser", new[] { "GamingEvent_GamingEventId" });
            DropIndex("dbo.ApplicationUserPost1", new[] { "Post_PostId" });
            DropIndex("dbo.ApplicationUserPost1", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.ApplicationUserCommentPost1", new[] { "CommentPost_CommentPostId" });
            DropIndex("dbo.ApplicationUserCommentPost1", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.ApplicationUserCommentEvent1", new[] { "CommentEvent_CommentEventId" });
            DropIndex("dbo.ApplicationUserCommentEvent1", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.ApplicationUserPost", new[] { "Post_PostId" });
            DropIndex("dbo.ApplicationUserPost", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.ApplicationUserCommentPost", new[] { "CommentPost_CommentPostId" });
            DropIndex("dbo.ApplicationUserCommentPost", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.ApplicationUserCommentEvent", new[] { "CommentEvent_CommentEventId" });
            DropIndex("dbo.ApplicationUserCommentEvent", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.ImageGroup", new[] { "GroupId" });
            DropIndex("dbo.ImageEvent", new[] { "GamingEventId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.Question", new[] { "Sender_Id" });
            DropIndex("dbo.Question", new[] { "GameId" });
            DropIndex("dbo.ImageGame", new[] { "GameId" });
            DropIndex("dbo.Game", new[] { "UserId" });
            DropIndex("dbo.Game", new[] { "CategoryGameId" });
            DropIndex("dbo.Content", new[] { "UserSender_Id" });
            DropIndex("dbo.Content", new[] { "ConversationId" });
            DropIndex("dbo.Conversation", new[] { "ReceiverId" });
            DropIndex("dbo.Conversation", new[] { "SenderId" });
            DropIndex("dbo.ImagePost", new[] { "PostId" });
            DropIndex("dbo.Post", new[] { "UserId" });
            DropIndex("dbo.Post", new[] { "CategoryPostId" });
            DropIndex("dbo.CommentPost", new[] { "PostId" });
            DropIndex("dbo.CommentPost", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.CommentEvent", new[] { "GamingEventId" });
            DropIndex("dbo.CommentEvent", new[] { "UserId" });
            DropIndex("dbo.GamingEvent", new[] { "Group_GroupId" });
            DropIndex("dbo.GamingEvent", new[] { "Publisher_Id" });
            DropIndex("dbo.GamingEvent", new[] { "CategoryEventId" });
            DropTable("dbo.GamingEventApplicationUser");
            DropTable("dbo.ApplicationUserPost1");
            DropTable("dbo.ApplicationUserCommentPost1");
            DropTable("dbo.ApplicationUserCommentEvent1");
            DropTable("dbo.ApplicationUserPost");
            DropTable("dbo.ApplicationUserCommentPost");
            DropTable("dbo.ApplicationUserCommentEvent");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.ImageGroup");
            DropTable("dbo.Group");
            DropTable("dbo.ImageEvent");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.Question");
            DropTable("dbo.ImageGame");
            DropTable("dbo.CategoryGame");
            DropTable("dbo.Game");
            DropTable("dbo.Content");
            DropTable("dbo.Conversation");
            DropTable("dbo.ImagePost");
            DropTable("dbo.CategoryPost");
            DropTable("dbo.Post");
            DropTable("dbo.CommentPost");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.CommentEvent");
            DropTable("dbo.GamingEvent");
            DropTable("dbo.CategoryEvent");
        }
    }
}
