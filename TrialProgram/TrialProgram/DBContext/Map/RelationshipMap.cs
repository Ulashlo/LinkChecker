using System.Data.Entity.ModelConfiguration;

namespace TrialProgram.DBContext.Map
{
    public class RelationshipMap : EntityTypeConfiguration<Relationship>
    {
        public RelationshipMap()
        {
            HasOptional(e => e.Parent)
                 .WithMany()
                 .HasForeignKey(e => e.ParentId);
            HasRequired(e => e.Child)
                .WithMany()
                .HasForeignKey(e => e.ChildId);
        }
    }
}
