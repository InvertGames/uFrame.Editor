using Invert.Core.GraphDesigner.Unity.WindowsPlugin;

namespace Invert.Core.GraphDesigner.Unity.WindowsSystem
{
    public class OpenWinCommand : EditorCommand<DiagramNodeViewModel>, IDiagramNodeCommand
    {
        public override string Group
        {
            get { return "OpenWinCommand"; }
        }

        public override decimal Order
        {
            get { return -1; }
        }

        public override bool CanProcessMultiple
        {
            get { return false; }
        }

        public override void Perform(DiagramNodeViewModel node)
        {
            InvertApplication.SignalEvent<IOpenWindow>(w => w.OpenWindow(node));
        }

        public override string CanPerform(DiagramNodeViewModel node)
        {
            var selected = node.DataObject as IDiagramNode;
            if (selected == null) return "Invalid argument";
            //if (node.IsExternal) return "Can't rename a node when its not local.";
            //if (selected.Graph.Identifier != InvertGraphEditor.CurrentDiagramViewModel.GraphData.Identifier)
            //  return "Must be local to rename.";
            return null;
        }


    }
}