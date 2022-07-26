using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toolbox.Core;

namespace SampleMapEditor
{
    [ByamlObject]
    public class DesignerObj : MuObj, IRailableParams, IByamlSerializable, IStageReferencable  //, IMusicLinkAnimationControllerParams /*Not Used*/
    {
        [ByamlMember]
        [BindGUI("Is Object Paint For Result", Category = "DesignerObj Properties")]
        public bool IsObjPaintForResult { get; set; }

        [ByamlMember] [BindGUI("Is Breakable By Large Enemy", Category = "DesignerObj Properties")] public bool IsBreakableByLargeEnemy { get; set; }
        
        [ByamlMember] [BindGUI("Is Appear After Clear In World", Category = "DesignerObj Properties")] public bool IsAppearAfterClearInWolrd { get; set; }


        // RAILABLE PARAMS
        [ByamlMember][BindGUI("Standby Frame", Category = "Railable Params")] public int RailableParams__StandbyFrame { get; set; }
        [ByamlMember][BindGUI("Move Frame", Category = "Railable Params")] public int RailableParams__MoveFrame { get; set; }
        [ByamlMember][BindGUI("Break Frame", Category = "Railable Params")] public int RailableParams__BreakFrame { get; set; }
        [ByamlMember][BindGUI("Patrol", Category = "Railable Params")] public int RailableParams__Patrol { get; set; }
        [ByamlMember][BindGUI("Interpolation", Category = "Railable Params")] public int RailableParams__Interpolation { get; set; }
        [ByamlMember][BindGUI("Velocity", Category = "Railable Params")] public int RailableParams__Vel { get; set; }
        [ByamlMember][BindGUI("Constant Velocity Unit Length", Category = "Railable Params")] public float RailableParams__ConstantVelUnitLength { get; set; }
        [ByamlMember][BindGUI("Att Calc", Category = "Railable Params")] public int RailableParams__AttCalc { get; set; }


        public DesignerObj() : base()
        {
            IsObjPaintForResult = false;
            IsBreakableByLargeEnemy = false;
            IsAppearAfterClearInWolrd = false;
        }
    }
}
