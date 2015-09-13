using Invert.uFrame.Editor.ViewModels;

namespace Invert.Core.GraphDesigner
{
    public class MoveDownCommand : EditorCommand<GenericNodeChildItem>, IDiagramNodeItemCommand, IKeyBindable
    {
        public override string Name
        {
            get { return "Move Down"; }
        }

        public override void Perform(GenericNodeChildItem node)
        {
            node.Node.MoveItemDown(node);
        }

        public override string CanPerform(GenericNodeChildItem node)
        {
            if (node != null) return null;
            return "Can't move item.";
        }
    }
}