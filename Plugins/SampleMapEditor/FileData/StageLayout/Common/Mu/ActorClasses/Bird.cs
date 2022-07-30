using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleMapEditor
{
    public class Bird : MuObj
    {
        [ByamlMember] public bool IsUseFlyRotY { get; set; }
        [ByamlMember] public float FlyRotY { get; set; }

        // THIS CODE WAS AUTO-GENERATED
    }
}