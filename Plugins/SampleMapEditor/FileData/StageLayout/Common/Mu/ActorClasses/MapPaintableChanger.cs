using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleMapEditor
{
    public class MapPaintableChanger : MuObj
    {
        [ByamlMember] public int ChangeType { get; set; }


        public MapPaintableChanger() : base()
        {
            ChangeType = 0;
        }

        public override MapPaintableChanger Clone()
        {
            MapPaintableChanger clone = (MapPaintableChanger)base.Clone();
            clone.ChangeType = ChangeType;
            return clone;
        }
    }
}
