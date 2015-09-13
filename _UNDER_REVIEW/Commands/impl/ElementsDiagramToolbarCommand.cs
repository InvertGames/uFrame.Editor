namespace Invert.Core.GraphDesigner
{
    public abstract class ElementsDiagramToolbarCommand : ToolbarCommand<DiagramViewModel>
    {
        public override string CanPerform(DiagramViewModel node)
        {
            if (node == null) return "No Diagram Open";
            return null;
        }
    }
}