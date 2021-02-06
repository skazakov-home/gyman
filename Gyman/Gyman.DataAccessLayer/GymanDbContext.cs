using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Gyman.BusinessLogicLayer;

namespace Gyman.DataAccessLayer
{
    public class GymanDbContext : DbContext
    {
        public GymanDbContext()
            : base("GymanDb")
        { }

        public virtual DbSet<Member> Members { get; set; }

        public virtual DbSet<Trainer> Trainers { get; set; }

        public virtual DbSet<Training> Trainings { get; set; }

        public virtual DbSet<Subscription> Subscriptions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
