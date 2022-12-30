using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toolbox.Core;

namespace SampleMapEditor
{
    public class Sponge : MuObj
    {
        [ByamlMember][BindGUI("Is Default Max", Category = "Sponge Properties")] public bool IsDefaultMax { get; set; }
        [ByamlMember][BindGUI("IsSteepPaint", Category = "Sponge Properties")] public bool IsSteepPaint { get; set; }
        [ByamlMember][BindGUI("IsCalcChargeBulletSpecialCol", Category = "Sponge Properties")] public bool IsCalcChargeBulletSpecialCol { get; set; }
        [ByamlMember][BindGUI("LinkedSponges", Category = "Sponge Properties")] public int LinkedSponges { get; set; }

        // THIS CODE WAS AUTO-GENERATED
    }
}
