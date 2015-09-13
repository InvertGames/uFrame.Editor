using System.Collections.Generic;

namespace Invert.Core.GraphDesigner
{
    public abstract class ContextMenuCommand<TFor> : EditorCommand<TFor> where TFor : class
    {
        public abstract IEnumerable<UFContextMenuItem> GetMenuOptions();
    }
}