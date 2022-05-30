using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleMapEditor
{
    public interface IRailFollowableParams
    {
        [ByamlMember] public int RailableParams__SolveType { get; set; }
    }
}
