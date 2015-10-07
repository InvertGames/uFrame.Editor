using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Invert.Data;
using Invert.Json;

namespace Invert.Core.GraphDesigner
{
    public interface IDiagramNodeItem : ISelectable, IJsonObject, IItem, IConnectable, IDataRecord
    {
        bool Precompiled { get; set; }
        string Name { get; set; }
        string Highlighter { get; }
        string FullLabel { get; }
        bool IsSelectable { get;}
        GraphNode Node { get; set; }
        [Browsable(false)]
        DataBag DataBag { get; set; }
        
        /// <summary>
        /// Is this node currently in edit mode/ rename mode.
        /// </summary>
        bool IsEditing { get; set; }

        bool this[string flag] { get; set; }

        string Namespace { get; }
        string NodeId { get; set; }


        //void Remove(IDiagramNode diagramNode);
        void Rename(IDiagramNode data, string name);
        void NodeRemoved(IDiagramNode nodeData);
        void NodeItemRemoved(IDiagramNodeItem nodeItem);
        void NodeAdded(IDiagramNode data);
        void NodeItemAdded(IDiagramNodeItem data);
        void Validate(List<ErrorInfo> info);
        void Document(IDocumentationBuilder docs);
        ErrorInfo[] Errors { get; set; }
        int Order { get; set; }
    }
}