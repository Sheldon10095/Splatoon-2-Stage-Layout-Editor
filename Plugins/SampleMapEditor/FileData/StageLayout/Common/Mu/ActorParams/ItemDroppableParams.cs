﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleMapEditor
{
    public interface ItemDroppableParams
    {
        [ByamlMember] public int ItemDropableParams__DropNum { get; set; }
    }
}
