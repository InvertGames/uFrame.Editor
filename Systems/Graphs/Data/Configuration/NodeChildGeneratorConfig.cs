using System;
using System.Collections.Generic;

namespace Invert.Core.GraphDesigner
{
    public class NodeChildGeneratorConfig<TNode> : NodeChildGeneratorConfigBase
    {
        public Func<TNode, IEnumerable<IGraphItem>> Selector { get; set; }
    }
}