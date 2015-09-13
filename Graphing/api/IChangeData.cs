using Invert.Json;

namespace Invert.Core.GraphDesigner
{
    public interface IChangeData : IJsonObject
    {
        bool IsValid { get; }
        IDiagramNodeItem Item { get; set; }
        string ItemIdentifier { get; set; }
        void Update(IChangeData data);
    }
}