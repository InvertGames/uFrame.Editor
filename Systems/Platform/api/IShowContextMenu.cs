namespace Invert.Core.GraphDesigner
{
    public interface IShowContextMenu
    {
        void Show(MouseEvent evt, params object[] objects);
    }
}