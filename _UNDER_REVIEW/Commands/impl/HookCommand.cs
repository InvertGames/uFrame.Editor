using System;

namespace Invert.Core.GraphDesigner
{
    public class HookCommand : EditorCommand
    {
        public HookCommand(Action action)
        {
            Action = action;
        }

        public override Type For
        {
            get { return typeof (DiagramViewModel); }
        }

        public Action Action { get; set; }

        public HookCommand()
        {
        }

        public override string CanPerform(object arg)
        {
            return null;
        }

        public override void Perform(object arg)
        {
            Action();
        }
    }
}