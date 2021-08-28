using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrialProgram
{
    public class Site
    {
        public int SiteId { get; set; }
        public string Uri { get; set; }
        public int NestingLevel { get; set; }

        public virtual ICollection<Link> Links { get; set; }
    }
}
