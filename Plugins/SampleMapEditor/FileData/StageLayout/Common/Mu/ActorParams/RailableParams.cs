using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleMapEditor
{
    public interface IRailableParams
    {
        [ByamlMember] public int RailableParams__StandbyFrame { get; set; }
        [ByamlMember] public int RailableParams__MoveFrame { get; set; }
        [ByamlMember] public int RailableParams__BreakFrame { get; set; }
        [ByamlMember] public int RailableParams__Patrol { get; set; }
        [ByamlMember] public int RailableParams__Interpolation { get; set; }
        [ByamlMember] public int RailableParams__Vel { get; set; }
        [ByamlMember] public float RailableParams__ConstantVelUnitLength { get; set; }
        [ByamlMember] public int RailableParams__AttCalc { get; set; }
    }
}
