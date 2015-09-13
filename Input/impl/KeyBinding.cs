using System;
using System.Text;
using Invert.Core.GraphDesigner;
using UnityEngine;
namespace Invert.Core.GraphDesigner
{ 
    public class KeyBinding<TCommandType> : IKeyBinding where TCommandType : class
    {
        public bool RequireShift { get; set; }
        public bool RequireAlt { get; set; }
        public bool RequireControl { get; set; }
        public string Name { get; set; }
        

        public KeyCode Key { get; set; }
        
        public KeyBinding(KeyCode key, bool requireControl = false, bool requireAlt = false, bool requireShift = false)
            : this(null, key, requireControl, requireAlt, requireShift)
        {

        }

        public KeyBinding(string name, KeyCode key, bool requireControl = false, bool requireAlt = false, bool requireShift = false)
        {
            Name = name;
            Key = key;
            RequireControl = requireControl;
            RequireAlt = requireAlt;
            RequireShift = requireShift;
        }

        public Type CommandType
        {
            get { return typeof(TCommandType); }
        }

        protected IEditorCommand _command;

        public virtual TCommandType CommandAsT
        {
            get
            {
                return (Command as TCommandType) ?? InvertGraphEditor.Container.Resolve<TCommandType>(Name);
            }
        }

        public IEditorCommand Command
        {
            get { return _command ?? InvertGraphEditor.Container.Resolve<TCommandType>(Name) as IEditorCommand; }
            set { _command = value; }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            if (RequireControl)
            {
                sb.Append("Ctrl + ");
            }
            if (RequireAlt)
            {
                sb.Append("Alt + ");
            }
            if (RequireShift)
            {
                sb.Append("Shift + ");
            }
            sb.Append(Key.ToString());
            return sb.ToString();
        }
    }
}