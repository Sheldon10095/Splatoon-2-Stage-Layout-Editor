using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleMapEditor
{
    public class PlazaJerryLocator : MuObj
    {
        [ByamlMember] public int Appear { get; set; }
        [ByamlMember] public int Movement { get; set; }
        [ByamlMember] public int NewsAppear { get; set; }
        [ByamlMember] public string AnimName { get; set; }
        [ByamlMember] public bool IsTankTop { get; set; }
        [ByamlMember] public int FixedClothIdx { get; set; }
        [ByamlMember] public bool IsFitToGnd { get; set; }

        // THIS CODE WAS AUTO-GENERATED
    }
}