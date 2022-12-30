using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toolbox.Core;

namespace SampleMapEditor
{
    public class Geyser : MuObj
    {
        [ByamlMember][BindGUI("Max Height", Category = "Geyser Properties")] public float MaxHeight { get; set; }

        // THIS CODE WAS AUTO-GENERATED
    }
}
