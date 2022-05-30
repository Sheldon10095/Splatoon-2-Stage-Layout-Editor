using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleMapEditor
{
    public class SpongeVersus : MuObj
    {
        [ByamlMember] public int Type { get; set; }
        [ByamlMember] public bool IsSteepPaint { get; set; }
        [ByamlMember] public bool IsPaintGnd { get; set; }
        [ByamlMember] public bool IsCalcChargeBulletSpecialCol { get; set; }
        [ByamlMember] public bool IsUseScaleMaxMu { get; set; }
        [ByamlMember] public float Scale_Max_Mu { get; set; }
        [ByamlMember] public bool IsUseScaleDamageForMaxMu { get; set; }
        [ByamlMember] public int ScaleDamageForMaxMu { get; set; }
        [ByamlMember] public int Reject { get; set; }
        [ByamlMember] public bool IsDefaultMax { get; set; }

        // THIS CODE WAS AUTO-GENERATED
    }
}
