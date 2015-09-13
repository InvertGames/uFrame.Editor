namespace Invert.Core.GraphDesigner
{
    public interface IElementFileData : INodeRepository
    {
        /// <summary>
        /// Should be called when first loaded.
        /// </summary>
        void Initialize();
    }
}