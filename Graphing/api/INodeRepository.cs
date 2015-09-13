using System.Collections.Generic;
using UnityEngine;

namespace Invert.Core.GraphDesigner
{
    public interface INodeRepository 
    {
#if UNITY_EDITOR
        string AssetPath { get; }
        string AssetDirectory { get; }

#endif
        string SystemDirectory { get; }
        string SystemPath { get; set; }
        // Basic Information
        string Name { get; set; }
        IEnumerable<IDiagramNode> NodeItems { get; }
        IEnumerable<IGraphItem> AllGraphItems { get; }
        //IEnumerable<ConnectionData> Connections { get; }

        // Settings
        ElementDiagramSettings Settings { get; }

        IGraphFilter CurrentFilter { get; }
        FilterPositionData PositionData { get; set; }
        string Namespace { get; set; }
        void RemoveItem(IDiagramNodeItem nodeItem);
        void AddItem(IDiagramNodeItem item);

        void Save(); 
        void RecordUndo(INodeRepository data, string title);
        void MarkDirty(INodeRepository data);
        void SetItemLocation(IDiagramNode node, Vector2 position);
        Vector2 GetItemLocation(IDiagramNode node);
        void HideNode(string identifier);
        IEnumerable<ErrorInfo> Validate();
    }
}