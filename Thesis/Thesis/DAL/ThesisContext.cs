using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using Thesis.Models;

namespace Thesis.DAL
{
    public class ThesisContext : IdentityDbContext<ApplicationUser>
    {
        public ThesisContext() : base("Thesis")
        {
        }

        //static ThesisContext()
        //{
        //    Database.SetInitializer<ThesisContext>(new ThesisInitializer());
        //}

        public static ThesisContext Create()
        {
            return new ThesisContext();
        }

        public DbSet<CategoryEvent> CategoryEvents { get; set; }
        public DbSet<CategoryGame> CategoryGames { get; set; }
        public DbSet<CategoryPost> CategoryPosts { get; set; }
        public DbSet<CommentEvent> CommentEvents { get; set; }
        public DbSet<CommentPost> CommentPosts { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<GamingEvent> GamingEvents { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<ImageEvent> ImageEvents { get; set; }
        public DbSet<ImageGame> ImageGames { get; set; }
        public DbSet<ImagePost> ImagePosts { get; set; }
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<Content> Contents { get; set; }
        public DbSet<Post> Posts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Do not create tables with 's' at the end of table name
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}