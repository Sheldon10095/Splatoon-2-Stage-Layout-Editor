using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleMapEditor
{
    public class PaintTargetArea : MuObj
    {
        [ByamlMember] public int SpecialAutoIncSec { get; set; }
        [ByamlMember] public bool IsOutlineOverWall { get; set; }

        // THIS CODE WAS AUTO-GENERATED


        public PaintTargetArea() : base()
        {
            SpecialAutoIncSec = 40;
            IsOutlineOverWall = false;
        }

        public override PaintTargetArea Clone()
        {
            PaintTargetArea clone = (PaintTargetArea)base.Clone();
            clone.SpecialAutoIncSec = SpecialAutoIncSec;
            clone.IsOutlineOverWall = IsOutlineOverWall;
            return clone;
        }
    }
}
