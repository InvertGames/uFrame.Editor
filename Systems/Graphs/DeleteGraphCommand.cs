namespace Invert.Core.GraphDesigner
{
    public class DeleteGraphCommand : ElementsDiagramToolbarCommand
    {
        public override string Name
        {
            get { return "Delete Graph"; }
        }

        public override ToolbarPosition Position
        {
            get { return ToolbarPosition.BottomRight; }
        }

        public override void Perform(DiagramViewModel node)
        {
            if (InvertGraphEditor.Platform.MessageBox("Are you sure?",
                "Are you sure you want to delete this graph, it will remove all nodes that belong to it?",
                "Yes Remove Them", "Cancel"))
            {
                node.CurrentRepository.Remove(node.GraphData);
            }
        }
    }
}