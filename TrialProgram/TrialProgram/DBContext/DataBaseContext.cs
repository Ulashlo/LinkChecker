using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using TrialProgram.DBContext.Map;
using TrialProgram.DBContext.Model;

namespace TrialProgram
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext() : base("MyConnection") { }

        public DbSet<Site> Sites { get; set; }
        public DbSet<Link> Linkes { get; set; }
        public DbSet<Relationship> Relationships { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Redirect> Redirects { get; set; }
        public DbSet<Exception> Exceptions { get; set; }
        public DbSet<Line> Lines { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Configurations.Add(new RelationshipMap());
            modelBuilder.Configurations.Add(new LinkMap());
        }
    }
}
