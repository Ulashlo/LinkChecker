using System.Data.Entity.ModelConfiguration;

namespace TrialProgram.DBContext.Map
{
    public class LinkMap : EntityTypeConfiguration<Link>
    {
        public LinkMap()
        {
            HasMany(e => e.Relationships)
                 .WithOptional()
                 .HasForeignKey(p => p.ParentId);
        }
    }
}
