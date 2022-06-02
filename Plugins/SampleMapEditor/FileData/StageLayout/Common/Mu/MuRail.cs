using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toolbox.Core;

namespace SampleMapEditor
{
    [ByamlObject]
    public class MuRail : MuElement /*, IByamlSerializable, IStageReferencable*/
    {
        [ByamlMember]
        public List<MuRailPoint> RailPoints { get; set; }


        [ByamlMember]
        [BindGUI("Is Closed")]
        public bool IsClosed { get; set; }


        [ByamlMember]
        [BindGUI("Rail Type")]
        public string RailType { get; set; }


        [ByamlMember]
        [BindGUI("Priority")]
        public int Priority { get; set; }



        // Methods

        public override void DeserializeByaml(IDictionary<string, object> dictionary)
        {
            base.DeserializeByaml(dictionary);
        }

        public override void SerializeByaml(IDictionary<string, object> dictionary)
        {
            base.SerializeByaml(dictionary);
        }

        public override void DeserializeReferences(StageDefinition stageDefinition)
        {
            base.DeserializeReferences(stageDefinition);
        }

        public override void SerializeReferences(StageDefinition stageDefinition)
        {
            base.SerializeReferences(stageDefinition);
        }
    }
}
