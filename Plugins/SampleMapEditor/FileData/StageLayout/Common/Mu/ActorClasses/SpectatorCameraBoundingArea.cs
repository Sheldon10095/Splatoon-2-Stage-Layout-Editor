using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleMapEditor
{
    public class SpectatorCameraBoundingArea : MuObj
    {
        [ByamlMember] public bool IsRoot { get; set; }

        // THIS CODE WAS AUTO-GENERATED

        public override SpectatorCameraBoundingArea Clone()
        {
            SpectatorCameraBoundingArea clone = (SpectatorCameraBoundingArea)base.Clone();
            clone.IsRoot = IsRoot;
            return clone;
        }
    }
}
