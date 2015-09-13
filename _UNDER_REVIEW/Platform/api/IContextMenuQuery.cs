namespace Invert.Core.GraphDesigner
{
    public interface IContextMenuQuery
    {
        void QueryContextMenu(ContextMenuUI ui, MouseEvent evt, object obj);
    }
}