using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleMapEditor
{
    public class VictoryClamSpawnPoint : MuObj
    {
        [ByamlMember] public bool IsGenMirror { get; set; }
        [ByamlMember] public bool IsGenMirror_AxisX { get; set; }

        // THIS CODE WAS AUTO-GENERATED


        public VictoryClamSpawnPoint() : base()
        {
            IsGenMirror = true;
            IsGenMirror_AxisX = false;
        }

        public override VictoryClamSpawnPoint Clone()
        {
            VictoryClamSpawnPoint clone = (VictoryClamSpawnPoint)base.Clone();
            clone.IsGenMirror = IsGenMirror;
            clone.IsGenMirror_AxisX = IsGenMirror_AxisX;
            return clone;
        }
    }
}
