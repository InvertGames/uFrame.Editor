using System;
using System.Collections.Generic;
using System.Linq;

namespace Invert.Core.GraphDesigner
{
    public abstract class EditorCommand :  IEditorCommand
    {
        private List<IEditorCommand> _hooks;
        protected string _title;

        public virtual bool CanProcessMultiple
        {
            get { return true; }
        }

        public abstract Type For { get; }

        public virtual void Execute(ICommandHandler handler)
        {
//#if UNITY_EDITOR
           
            foreach (var item in GetCommandObjects(handler))
            {
                var item1 = item;
                InvertApplication.SignalEvent<ICommandEvents>(_ => _.CommandExecuting(handler, this, item1));
                handler.CommandExecuting(this);
                Perform(item);
                handler.CommandExecuted(this);
                InvertApplication.SignalEvent<ICommandEvents>(_ => _.CommandExecuted(handler, this, item1));

                if (!CanProcessMultiple)
                    break;
               
            }
            
//#else
//            // IN wpf this graph editor will invoke perform, not this method
//            InvertGraphEditor.ExecuteCommand(this);
//#endif
        }

        protected virtual IEnumerable<object> GetCommandObjects(ICommandHandler handler)
        {
            var f = For;
            return handler.ContextObjects.Where(p => p != null && f.IsAssignableFrom(p.GetType())).ToArray();
        }

        public event EventHandler CanExecuteChanged;

        public abstract void Perform(object arg);
        [Obsolete]
        public List<IEditorCommand> Hooks
        {
            get { return _hooks ?? (_hooks = new List<IEditorCommand>()); }
            set { _hooks = value; }
        }

        public virtual string Name
        {
            get { return this.GetType().Name.Replace("Command",""); }
        }

        public virtual string Title
        {
            get
            {
                if (string.IsNullOrEmpty(_title))
                {
                    return Name;
                }
                return _title;
            }
            set { _title = value; }
        }

        public string CanExecute(object arg)
        {
            return null;
        }

        public virtual decimal Order { get { return 0; } }
        public virtual bool ShowAsDiabled { get { return false; } }
        public virtual string Group { get { return "Default"; }}

        public virtual string CanExecute(ICommandHandler handler)
        {
            var objs = GetCommandObjects(handler).ToArray();

            if (objs.Length > 1 && !CanProcessMultiple) 
                return "This command can't be performed on multiple objects.";
            if (objs.Length == 0)
                return "Nothing to execute this command on";

            foreach (var obj in objs)
            {
                var result = CanPerform(obj);
                if (!string.IsNullOrEmpty(result)) return result;
            }
            return null;
        }

        public abstract string CanPerform(object arg);
        public virtual string Path { get { return Name; } }

        public virtual bool IsChecked(object arg)
        {
            return false;
        }

        public virtual IKeyBinding GetKeyBinding()
        {
            return null;
        } 
    }
    
}