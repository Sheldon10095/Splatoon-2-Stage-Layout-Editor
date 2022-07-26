using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toolbox.Core;

namespace SampleMapEditor
{
    public class SpongeVersus : MuObj
    {
        [ByamlMember] [BindGUI("Sponge Type", Category = "Sponge Properties")] public int Type { get; set; }
        [ByamlMember] [BindGUI("IsSteepPaint", Category = "Sponge Properties")] public bool IsSteepPaint { get; set; }
        [ByamlMember] [BindGUI("IsPaintGround", Category = "Sponge Properties")] public bool IsPaintGnd { get; set; }
        [ByamlMember] [BindGUI("IsCalcChargeBulletSpecialCol", Category = "Sponge Properties")] public bool IsCalcChargeBulletSpecialCol { get; set; }
        [ByamlMember] [BindGUI("IsUseScaleMaxMu", Category = "Sponge Properties")] public bool IsUseScaleMaxMu { get; set; }
        [ByamlMember] [BindGUI("Scale_Max_Mu", Category = "Sponge Properties")] public float Scale_Max_Mu { get; set; }
        [ByamlMember] [BindGUI("IsUseScaleDamageForMaxMu", Category = "Sponge Properties")] public bool IsUseScaleDamageForMaxMu { get; set; }
        [ByamlMember] [BindGUI("ScaleDamageForMaxMu", Category = "Sponge Properties")] public int ScaleDamageForMaxMu { get; set; }
        [ByamlMember] [BindGUI("Reject", Category = "Sponge Properties")] public int Reject { get; set; }
        [ByamlMember] [BindGUI("Is Default Max", Category = "Sponge Properties")] public bool IsDefaultMax { get; set; }

        // THIS CODE WAS AUTO-GENERATED
    }
}
