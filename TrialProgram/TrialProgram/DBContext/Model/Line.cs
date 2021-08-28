using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrialProgram.DBContext.Model
{
   public class Line
    {
        public int LineId { get; set; }
        public int RelationshipId { get; set; }
        public int LineNumber { get; set; }

        public virtual Relationship Relationship { get; set; }
    }
}
