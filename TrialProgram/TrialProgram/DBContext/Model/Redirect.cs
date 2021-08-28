using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrialProgram
{
    public class Redirect
    {
        public int RedirectId { get; set; }
        public int LinkId { get; set; }
        public string LinkStr { get; set; }
        public int AnswerId { get; set; }
        public virtual Link Link { get; set; }
        public virtual Answer Answer { get; set; }
    }
}
