using Invert.Core.GraphDesigner;
using Invert.Core.GraphDesigner;

public class GenericGraphData<T> : InvertGraph where T : IGraphFilter, new()
{
    public T FilterNode
    {
        get { return (T)RootFilter; }
    }

    public override IGraphFilter CreateDefaultFilter()
    {
        var filterItem = new T()
        {
            
        };
        Repository.Add(filterItem);
        var item = new FilterItem();
        item.NodeId = filterItem.Identifier;
        item.FilterId = filterItem.Identifier;
        Repository.Add(item);
        return filterItem;
    }
}