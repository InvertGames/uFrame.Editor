namespace Invert.Core.GraphDesigner
{
    public abstract class ToolbarCommand<TFor> : EditorCommand<TFor>, IToolbarCommand where TFor : class
    {
        public virtual ToolbarPosition Position { get { return ToolbarPosition.Right; } }
        public virtual decimal Order { get { return 0; } }

        
    }
}