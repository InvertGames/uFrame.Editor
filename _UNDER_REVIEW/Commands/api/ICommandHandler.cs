using System.Collections.Generic;

namespace Invert.Core.GraphDesigner
{
    public interface ICommandHandler
    {
        IEnumerable<object> ContextObjects { get; }
        void CommandExecuted(IEditorCommand command);
        void CommandExecuting(IEditorCommand command);

    }
}