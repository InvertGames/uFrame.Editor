namespace Invert.Core.GraphDesigner
{
    public class NodeHelpCommand : EditorCommand<GraphNode>, IDiagramNodeCommand
    {
        public override string Name
        {
            get { return "Help and Documentation"; }
        }

        public override string Path
        {
            get { return "Help and Documentation"; }
        }

        public override void Perform(GraphNode node)
        {
            // TODO 2.0 This was confusing
            //InvertGraphEditor.WindowManager.ShowHelpWindow(node.Project.CurrentGraph.RootFilter.GetType().Name, node.GetType());
        }

        public override string CanPerform(GraphNode node)
        {
            if (node == null)
            {
                return "Show a diagram first.";
            }
            return null;
        }
    }
}