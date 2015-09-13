using System;
using System.Collections.Generic;

namespace Invert.Core.GraphDesigner
{
    public class SelectTypeCommand : Command
    {
        public Func<IDiagramNodeItem, GraphTypeInfo[]> TypesSelector { get; set; }
        private List<GraphTypeInfo> _additionalTypes;
        public bool AllowNone { get; set; }
        public bool PrimitiveOnly { get; set; }
        public bool IncludePrimitives { get; set; }
        public List<GraphTypeInfo> AdditionalTypes
        {
            get { return _additionalTypes ?? (_additionalTypes = new List<GraphTypeInfo>()); }
            set { _additionalTypes = value; }
        }

        public TypedItemViewModel ItemViewModel { get; set; }
    }
}