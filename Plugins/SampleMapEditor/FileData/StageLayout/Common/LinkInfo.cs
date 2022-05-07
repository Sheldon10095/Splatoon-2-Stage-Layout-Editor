using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleMapEditor
{
    public class LinkInfo
    {
        [ByamlMember]
        public string DefinitionName { get; set; }

        [ByamlMember]
        public string DestUnitId { get; set; }
    }
}
