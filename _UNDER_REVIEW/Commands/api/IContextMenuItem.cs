namespace Invert.Core.GraphDesigner
{
    public interface IContextMenuItem
    {
        string Path { get; }
        bool IsChecked(object arg);
    }
}