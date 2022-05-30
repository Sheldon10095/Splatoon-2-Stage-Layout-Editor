using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleMapEditor
{
    public class Jerry : MuObj
    {
        [ByamlMember] public string Anim0 { get; set; }
        [ByamlMember] public int AnimStartFrame { get; set; }
        [ByamlMember] public bool IsEnableOverlookFade { get; set; }
        [ByamlMember] public int ClothAnimFrame { get; set; }
        [ByamlMember] public bool IsOceanBind { get; set; }

        // THIS CODE WAS AUTO-GENERATED
    }
}
