namespace Invert.Core.GraphDesigner
{
    public class NavigateToNodeCommand : Command
    {
        public IDiagramNode Node;
        public bool Select { get; set; }
    }
}