using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialProgram.DBContext.Model;

namespace TrialProgram
{
    public class Relationship
    {
        [Key]
        public int RelationshipId { get; set; }
        public int? ParentId { get; set; }
        public int ChildId { get; set; }

        public virtual ICollection<Line> Lines { get; set; }
        public virtual Link Parent { get; set; }
        public virtual Link Child { get; set; }

    }
}
