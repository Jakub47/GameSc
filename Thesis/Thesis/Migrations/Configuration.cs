namespace Thesis.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Thesis.DAL;

    public sealed class Configuration : DbMigrationsConfiguration<Thesis.DAL.ThesisContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Thesis.DAL.ThesisContext context)
        {
            //  This method will be called after migrating to the latest version.
            ThesisInitializer.DataSeed(context);
            ThesisInitializer.SeedUsers(context);
            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}
