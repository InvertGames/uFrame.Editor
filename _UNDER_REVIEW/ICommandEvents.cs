namespace Invert.Core.GraphDesigner
{
    public interface ICommandEvents
    {
        void CommandExecuting(ICommandHandler handler, IEditorCommand command, object o);
        void CommandExecuted(ICommandHandler handler, IEditorCommand command, object o);
    }
}