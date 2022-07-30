using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleMapEditor
{
    public class PlazaDoor : MuObj
    {
        [ByamlMember] public string SceneName { get; set; }
        [ByamlMember] public string ShopName { get; set; }
        [ByamlMember] public bool IsEnableEve { get; set; }

        // THIS CODE WAS AUTO-GENERATED
    }
}
