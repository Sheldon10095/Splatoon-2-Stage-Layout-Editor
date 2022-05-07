namespace SampleMapEditor
{
    /// <summary>
    /// Represents a BYAML element which references others and thus must resolve and build the dependencies.
    /// </summary>
    public interface IStageReferencable
    {
        // ---- METHODS ------------------------------------------------------------------------------------------------

        /// <summary>
        /// Allows references of course data objects to be resolved to provide real instances instead of the raw values
        /// in the BYAML.
        /// </summary>
        /// <param name="stageDefinition">The <see cref="StageDefinition"/> providing the objects.</param>
        void DeserializeReferences(StageDefinition stageDefinition);

        /// <summary>
        /// Allows references between course objects to be serialized into raw values stored in the BYAML.
        /// </summary>
        /// <param name="stageDefinition">The <see cref="StageDefinition"/> providing the objects.</param>
        void SerializeReferences(StageDefinition stageDefinition);
    }
}
