using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleMapEditor
{
    public class VictoryClamBankEmitArea : MuObj
    {
        [ByamlMember] public int Weight { get; set; }

        // THIS CODE WAS AUTO-GENERATED

        public VictoryClamBankEmitArea() : base()
        {
            Weight = 1;
        }

        public override VictoryClamBankEmitArea Clone()
        {
            VictoryClamBankEmitArea clone = (VictoryClamBankEmitArea)base.Clone();
            clone.Weight = Weight;
            return clone;
        }
    }
}
