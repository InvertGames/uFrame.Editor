using Invert.uFrame.Editor.ViewModels;

namespace Invert.Core.GraphDesigner
{
    public class ImportCommand : EditorCommand<DiagramNodeViewModel>, IDiagramContextCommand
    {
        public override void Perform(DiagramNodeViewModel node)
        {

        }

        public override string CanPerform(DiagramNodeViewModel node)
        {
            //if (node.ExportGraphType == null) return "This node is not exportable.";
            if (node.IsLocal) return "Node must be external to import it.";
            if (node.GraphItemObject is IGraphFilter) return null;
            return "Node must be a filter to export it.";
        }
    }
}