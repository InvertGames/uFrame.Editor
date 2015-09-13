namespace Invert.Core.GraphDesigner
{
    public interface IToolbarCommand : ICommand
    {
        ToolbarPosition Position { get; }
    }
}