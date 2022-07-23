using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toolbox.Core;

namespace SampleMapEditor
{
    [ByamlObject]
    public class MuObj : MuElement, IByamlSerializable, IStageReferencable
    {
        [ByamlMember]
        [BindGUI("Team", Category = "Object Properties")]
        public int Team { get; set; }


        public MuObj() : base()
        {
            Team = 0;
        }

        public override MuObj Clone()
        {
            MuObj cloned = (MuObj)base.Clone();
            cloned.Team = this.Team;
            return cloned;
        }



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
