using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrialProgram
{
    public class Link
    {
        public int LinkId { get; set; }
        public int SiteId { get; set; }
        public string Uri { get; set; }
        public string OriginalUri { get; set; }
        public int? AnswerId { get; set; }
        public bool IsInner { get; set; }
        public int NestingLevel { get; set; }
        public int? ExceptionId { get; set; }

        public virtual ICollection<Redirect> Redirects { get; set; }
        public virtual Site Site { get; set; }
        public virtual Answer Answer { get; set; }
        public virtual Exception Exception { get; set; }
        public virtual ICollection<Relationship> Relationships{ get; set; }

    }
}
