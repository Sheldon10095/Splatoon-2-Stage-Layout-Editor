using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleMapEditor
{
    [ByamlObject]
    public class MuRailPoint : MuElement
    {
        [ByamlMember]
        public int CheckPointIndex { get; set; }


        [ByamlMember]
        public List<ByamlVector3F> ControlPoints { get; set; }  // = new List<ByamlVector3F>();


        [ByamlMember]
        public float OffsetX { get; set; }


        [ByamlMember]
        public float OffsetY { get; set; }


        [ByamlMember]
        public float OffsetZ { get; set; }


        [ByamlMember]
        public bool UseOffset { get; set; }




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
