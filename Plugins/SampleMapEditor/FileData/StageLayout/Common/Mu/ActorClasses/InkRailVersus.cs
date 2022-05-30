using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleMapEditor
{
    public class InkRailVersus : MuObj, ISwitchableParams
    {
        [ByamlMember] public int Behaviour { get; set; }
        [ByamlMember] public bool IsAbleToBeCulled { get; set; }
        [ByamlMember] public int ConnectionTime_Coop { get; set; }

        // ~~~ Switchable Params ~~~
        [ByamlMember] public int SwitchableParams__InitialState { get; set; }
        [ByamlMember] public int SwitchableParams__RespawnReset { get; set; }

        // THIS CODE WAS AUTO-GENERATED


        public InkRailVersus() : base()
        {
            Behaviour = 0;
            IsAbleToBeCulled = false;
            ConnectionTime_Coop = 600;

            SwitchableParams__InitialState = 0;
            SwitchableParams__RespawnReset = 2;
        }

        public override InkRailVersus Clone()
        {
            InkRailVersus clone = (InkRailVersus)base.Clone();
            clone.Behaviour = Behaviour;
            clone.IsAbleToBeCulled = IsAbleToBeCulled;
            clone.ConnectionTime_Coop = ConnectionTime_Coop;
            clone.SwitchableParams__InitialState = SwitchableParams__InitialState;
            clone.SwitchableParams__RespawnReset = SwitchableParams__RespawnReset;
            return clone;
        }
    }
}
