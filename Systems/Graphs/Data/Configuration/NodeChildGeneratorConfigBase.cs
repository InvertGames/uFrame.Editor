using System;

namespace Invert.Core.GraphDesigner
{

    public class NodeChildGeneratorConfigBase
    {
        public virtual Type ChildType { get; set; }
        public IMemberGenerator Generator { get; set; }

    }

}