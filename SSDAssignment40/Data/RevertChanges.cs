using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSDAssignment40.Data
{
    public class RevertChanges
    {
        public string RevertChangesId { get; set; }
        public virtual Lodger OldLodgerUser { get; set; }
    }
}
