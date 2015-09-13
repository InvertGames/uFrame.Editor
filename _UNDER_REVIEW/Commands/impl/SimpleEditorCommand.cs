using System;
using Invert.Core.GraphDesigner;

namespace Invert.Core.GraphDesigner
{
    public class SimpleEditorCommand<TFor> : EditorCommand<TFor> where TFor : class
    {
        private string _name;
        private string _group;
        public override string Name
        {
            get { return _name ?? base.Name; }
        }

        public SimpleEditorCommand(Action<TFor> performAction,string name = null,string group = null)
        {
            PerformAction = performAction;
            _name = name;
            _title = name;
            _group = group;
        }

        public override string Group
        {
            get { return _group ?? base.Group; }
        }

        public Action<TFor> PerformAction { get; set; }

        public override string CanPerform(TFor arg)
        {
            return null;
        }

        public override void Perform(TFor arg)
        {
            this.PerformAction(arg);
        }
    }
}