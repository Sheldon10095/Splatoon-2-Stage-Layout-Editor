using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toolbox.Core;

namespace SampleMapEditor
{
    public class VictoryClamBasket : MuObj
    {
        [ByamlMember] [BindGUI("HeightBarrierCenter", Category = "Clam Basket Properties")] public float HeightBarrierCenter { get; set; }
        [ByamlMember] [BindGUI("Clam Supply Speed", Category = "Clam Basket Properties")] public float ClamSupplySpeed { get; set; }
        [ByamlMember] [BindGUI("DegAxisY_SpawnFromGoal", Category = "Clam Basket Properties")] public float DegAxisY_SpawnFromGoal { get; set; }
        [ByamlMember] [BindGUI("AutoGen Opposite Barrier? (Team must be Bravo!)", Category = "Clam Basket Properties")] public bool IsAutoGenOpposite { get; set; }

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
