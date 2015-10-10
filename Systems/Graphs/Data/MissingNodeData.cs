using System.Collections.Generic;
using Invert.Core.GraphDesigner;
using Invert.Json;

public class MissingNodeData : GraphNode
{
    public JSONClass _CachedData;
    public override void Deserialize(JSONClass cls)
    {
        //base.Deserialize(cls, repository);
        _CachedData = cls;
    }

    public override void NodeItemRemoved(IDiagramNodeItem item)
    {
        
    }

    public override void Serialize(JSONClass cls)
    {
        //base.Serialize(cls);
        foreach (KeyValuePair<string,JSONNode> item in _CachedData)
        {
            cls.Add(item.Key,item.Value);
        }
    }

    public override IEnumerable<IDiagramNodeItem> DisplayedItems
    {
        get { yield break; }
    }

    public override string Label
    {
        get { return "Missing Type " + _CachedData["_CLRType"].Value; }
    }

    public override string Name
    {
        get { return Label; }

    }

    public override IEnumerable<IDiagramNodeItem> PersistedItems
    {
        get { yield break; }
        set {  }
    }
}