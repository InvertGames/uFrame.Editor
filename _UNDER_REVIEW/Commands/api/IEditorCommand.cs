using System;
using System.Collections.Generic;

namespace Invert.Core.GraphDesigner
{
    public interface IEditorCommand :
#if !UNITY_EDITOR
    IContextMenuItemCommand
#else
         IContextMenuItemCommand
#endif
    {
        Type For { get; }
        void Execute(ICommandHandler handler);
        List<IEditorCommand> Hooks { get; }
        string Name { get; } 
        string Title { get; set; }
        decimal Order { get; }
        bool ShowAsDiabled { get; }
        string Group { get;  }
        string CanPerform(object arg);
#if !UNITY_EDITOR
        void Perform(object arg);
#endif

        IKeyBinding GetKeyBinding();
        string CanExecute(ICommandHandler handler);
    }
}