using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleMapEditor
{
    public class DesignerObj : MuObj, IRailableParams //, IMusicLinkAnimationControllerParams /*Not Used*/
    {
        [ByamlMember] public bool IsObjPaintForResult { get; set; }
        [ByamlMember] public bool IsBreakableByLargeEnemy { get; set; }
        [ByamlMember] public bool IsAppearAfterClearInWolrd { get; set; }


        // RAILABLE PARAMS
        [ByamlMember] public int RailableParams__StandbyFrame { get; set; }
        [ByamlMember] public int RailableParams__MoveFrame { get; set; }
        [ByamlMember] public int RailableParams__BreakFrame { get; set; }
        [ByamlMember] public int RailableParams__Patrol { get; set; }
        [ByamlMember] public int RailableParams__Interpolation { get; set; }
        [ByamlMember] public int RailableParams__Vel { get; set; }
        [ByamlMember] public float RailableParams__ConstantVelUnitLength { get; set; }
        [ByamlMember] public int RailableParams__AttCalc { get; set; }


        public DesignerObj() : base()
        {
            IsObjPaintForResult = false;
            IsBreakableByLargeEnemy = false;
            IsAppearAfterClearInWolrd = false;
        }
    }
}
