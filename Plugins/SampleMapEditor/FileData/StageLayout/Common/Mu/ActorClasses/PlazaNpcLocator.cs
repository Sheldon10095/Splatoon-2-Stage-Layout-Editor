using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleMapEditor
{
    public class PlazaNpcLocator : MuObj
    {
        [ByamlMember] public int Appear { get; set; }
        [ByamlMember] public string AnimName { get; set; }
        [ByamlMember] public int Movement { get; set; }
        [ByamlMember] public int Match { get; set; }
        [ByamlMember] public float TalkBaseDir { get; set; }
        [ByamlMember] public float TalkDirRange { get; set; }
        [ByamlMember] public float TalkExclusionDirRange { get; set; }
        [ByamlMember] public bool UseNewsOffset { get; set; }
        [ByamlMember] public float NewsOffset__X { get; set; }
        [ByamlMember] public float NewsOffset__Y { get; set; }
        [ByamlMember] public float NewsOffset__Z { get; set; }
        [ByamlMember] public float NewsRotY { get; set; }
        [ByamlMember] public bool IsFitToGnd { get; set; }

        // THIS CODE WAS AUTO-GENERATED
    }
}