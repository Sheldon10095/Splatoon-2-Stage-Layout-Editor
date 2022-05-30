using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleMapEditor
{
    public interface ISwitchableParams
    {
        [ByamlMember] public int SwitchableParams__InitialState { get; set; }
        [ByamlMember] public int SwitchableParams__RespawnReset { get; set; }
    }
}
