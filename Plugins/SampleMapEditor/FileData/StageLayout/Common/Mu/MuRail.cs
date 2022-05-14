using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleMapEditor
{
    [ByamlObject]
    public class MuRail : MuElement /*, IByamlSerializable, IStageReferencable*/
    {
        [ByamlMember]
        public List<MuRailPoint> RailPoints { get; set; }


        [ByamlMember]
        public bool IsClosed { get; set; }


        [ByamlMember]
        public string RailType { get; set; }


        [ByamlMember]
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
