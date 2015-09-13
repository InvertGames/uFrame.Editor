namespace Invert.Core.GraphDesigner
{
    public class HelpCommand : ToolbarCommand<DiagramViewModel>
    {
        public override ToolbarPosition Position
        {
            get { return ToolbarPosition.BottomRight; }
        }

        public override void Perform(DiagramViewModel node)
        {
            InvertGraphEditor.WindowManager.ShowHelpWindow(node.GraphData.RootFilter.GetType().Name,null);
        }

        public override string CanPerform(DiagramViewModel node)
        {
            if (node == null)
            {
                return "Show a diagram first.";
            }
            return null;
        }
    }
}