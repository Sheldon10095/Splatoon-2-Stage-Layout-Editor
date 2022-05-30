using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleMapEditor
{
    public class Gachihoko : MuObj
    {
        [ByamlMember] public float HikikomoriDetectionRate { get; set; }

        // THIS CODE WAS AUTO-GENERATED

        public override Gachihoko Clone()
        {
            Gachihoko clone = (Gachihoko)base.Clone();
            clone.HikikomoriDetectionRate = HikikomoriDetectionRate;
            return clone;
        }
    }
}
