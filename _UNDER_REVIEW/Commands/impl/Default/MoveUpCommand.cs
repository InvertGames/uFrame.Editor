using Invert.uFrame.Editor.ViewModels;

namespace Invert.Core.GraphDesigner
{
    public class MoveUpCommand : EditorCommand<GenericNodeChildItem>, IDiagramNodeItemCommand, IKeyBindable
    {
        public override string Name
        {
            get { return "Move Up"; }
        }
        public override void Perform(GenericNodeChildItem node)
        {
            node.Node.MoveItemUp(node);
        }

        public override string CanPerform(GenericNodeChildItem node)
        {
            if (node != null) return null;
            return "Can't move item.";
        }
    }
}