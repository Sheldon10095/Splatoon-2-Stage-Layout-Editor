using System.Collections.Generic;
using System.Diagnostics;

namespace SampleMapEditor
{
    /// <summary>
    /// Represents an object in the course which can be referenced by its <see cref="Id"/>.
    /// </summary>
    [ByamlObject]
    [DebuggerDisplay("{GetType().Name}  Id={Id}")]
    public abstract class UnitObject
    {
        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        /// <summary>
        /// Gets or sets a number identifying this object. Can be non-unique or 0 without any issues.
        /// </summary>
        [ByamlMember]
        public string Id { get; set; }

        //[ByamlMember]
        //public int UnitIdNum { get; set; }
    }
}
