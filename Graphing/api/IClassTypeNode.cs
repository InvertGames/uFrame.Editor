using System;

namespace Invert.Core.GraphDesigner
{
    [Obsolete]
    public interface IClassTypeNode : IDiagramNodeItem
    {
        string ClassName { get; }
    }

    /// <summary>
    /// Tag this interface so that it is validated with all other class nodes, ensuring the name doesn't conflict
    /// </summary>
    public interface IClassNode : IDiagramNodeItem, ITypeInfo
    {
        
    }
}