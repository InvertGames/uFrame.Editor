using Invert.uFrame.Editor.ViewModels;

namespace Invert.Core.GraphDesigner
{
    public class RemoveNodeItemCommand : EditorCommand<DiagramViewModel>, IDiagramNodeItemCommand
    {
        public override string Name
        {
            get { return "Remove Item"; }
        }


        public override bool IsChecked(DiagramViewModel arg)
        {
            return false;
        }

        public override void Perform(DiagramViewModel arg)
        {
            var diagramNodeItem = arg.SelectedNodeItem as ItemViewModel;
            if (diagramNodeItem != null)
                arg.CurrentRepository.Remove(diagramNodeItem.NodeItem);
        }

        public override string CanPerform(DiagramViewModel node)
        {
            return null;
        }
    }
}