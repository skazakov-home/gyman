using System.Data.Entity.Migrations;

namespace Gyman.DataAccessLayer.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<GymanDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(GymanDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
        }
    }
}
