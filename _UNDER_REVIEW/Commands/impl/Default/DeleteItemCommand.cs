using Invert.Data;
using Invert.uFrame.Editor.ViewModels;

namespace Invert.Core.GraphDesigner
{
    public class DeleteItemCommand : EditorCommand<DiagramNodeItem>, IDiagramNodeItemCommand, IKeyBindable
    {
        public override string Name
        {
            get { return "Delete"; }
        }

        public override void Perform(DiagramNodeItem node)
        {
            InvertApplication.Log("Deleting Item");
            if (node == null) return;
          
            var project = node.Node.Repository;
            project.Remove(node);
     
        }

        public override string CanPerform(DiagramNodeItem node)
        {
           
            //if (node is GenericSlot) return "Can't delete a slot";
            return null;
        }
    }
}