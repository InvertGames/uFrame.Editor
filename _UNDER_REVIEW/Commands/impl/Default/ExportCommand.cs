namespace Invert.Core.GraphDesigner
{
    public class ExportCommand : EditorCommand<DiagramNodeViewModel>, IDiagramNodeCommand
    {
        public override string Group
        {
            get { return "Moving"; }
        }
        public override void Perform(DiagramNodeViewModel node)
        {
            var diagramViewModel = node.DiagramViewModel;
            var nodeData = node.GraphItemObject as IDiagramNode;
            var repository = diagramViewModel.CurrentRepository;
            var filter = nodeData as IGraphFilter;
            // TODO fix up exporting
            //repository.ExportNode(node.ExportGraphType,diagramViewModel.DiagramData,node.DataObject as IDiagramNode,true);

        }

        public override string CanPerform(DiagramNodeViewModel node)
        {
            if (!node.IsLocal) return "Node must be local to export it.";
            if (node.GraphItemObject is IGraphFilter) return null;
            if (node.ExportGraphType == null) return null;
            return "Node must be a filter to export it.";
        }
    }
}