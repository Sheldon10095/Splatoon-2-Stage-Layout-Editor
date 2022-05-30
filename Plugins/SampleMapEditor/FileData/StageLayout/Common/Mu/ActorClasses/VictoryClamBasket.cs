using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleMapEditor
{
    public class VictoryClamBasket : MuObj
    {
        [ByamlMember] public float HeightBarrierCenter { get; set; }
        [ByamlMember] public float ClamSupplySpeed { get; set; }
        [ByamlMember] public float DegAxisY_SpawnFromGoal { get; set; }
        [ByamlMember] public bool IsAutoGenOpposite { get; set; }

        // THIS CODE WAS AUTO-GENERATED


        public VictoryClamBasket() : base()
        {
            HeightBarrierCenter = 75;
            ClamSupplySpeed = 1;
            DegAxisY_SpawnFromGoal = 0;
            IsAutoGenOpposite = true;
        }

        public override VictoryClamBasket Clone()
        {
            VictoryClamBasket clone = (VictoryClamBasket)base.Clone();
            clone.HeightBarrierCenter = HeightBarrierCenter;
            clone.ClamSupplySpeed = ClamSupplySpeed;
            clone.DegAxisY_SpawnFromGoal = DegAxisY_SpawnFromGoal;
            clone.IsAutoGenOpposite = IsAutoGenOpposite;
            return clone;
        }
    }
}
