using System;
using Invert.Core.GraphDesigner;
using UnityEngine;

namespace Invert.uFrame.Editor
{
    public class SimpleKeyBinding : KeyBinding<IEditorCommand>
    {

        //public SimpleKeyBinding(Action<DiagramDrawer> command, KeyCode key, bool requireControl = false, bool requireAlt = false, bool requireShift = false)
        //    : base(key, requireControl, requireAlt, requireShift)
        //{
        //    _command = new SimpleEditorCommand<DiagramDrawer>(command);
        //}

        //public SimpleKeyBinding(Action<DiagramDrawer> command, string name, KeyCode key, bool requireControl = false, bool requireAlt = false, bool requireShift = false)
        //    : base(name, key, requireControl, requireAlt, requireShift)
        //{
        //    _command = new SimpleEditorCommand<DiagramDrawer>(command);
        //}
        public SimpleKeyBinding(IEditorCommand command, KeyCode key, bool requireControl = false, bool requireAlt = false, bool requireShift = false)
            : base(key, requireControl, requireAlt, requireShift)
        {
            _command = command;
        }

        public SimpleKeyBinding(IEditorCommand command, string name, KeyCode key, bool requireControl = false, bool requireAlt = false, bool requireShift = false)
            : base(name, key, requireControl, requireAlt, requireShift)
        {
            _command = command;
        }


    }
}