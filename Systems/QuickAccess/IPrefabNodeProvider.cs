using System.Collections.Generic;

namespace Invert.Core.GraphDesigner
{
    public interface IPrefabNodeProvider
    {
        IEnumerable<QuickAddItem> PrefabNodes(INodeRepository nodeRepository);
    }
}