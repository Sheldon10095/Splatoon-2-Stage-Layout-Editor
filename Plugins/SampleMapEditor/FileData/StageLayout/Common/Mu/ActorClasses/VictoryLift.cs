using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleMapEditor
{
    public class VictoryLift : MuObj, IRailFollowableParams
    {
        [ByamlMember] public int Height { get; set; }
        [ByamlMember] public float YaguraOffsetY { get; set; }
        [ByamlMember] public bool IsGoalReverse { get; set; }
        [ByamlMember] public bool PlayerInclusionRejectWeak { get; set; }

        // ~~~ Rail Followable Params ~~~
        [ByamlMember] public int RailableParams__SolveType { get; set; }

        // THIS CODE WAS AUTO-GENERATED
    }
}
