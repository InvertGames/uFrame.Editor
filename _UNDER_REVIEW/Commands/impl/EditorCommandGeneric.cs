using System;

namespace Invert.Core.GraphDesigner
{
    public abstract class EditorCommand<TFor> : EditorCommand, IDiagramContextCommand, IDiagramNodeCommand where TFor : class
    {
        public override Type For
        {
            get { return typeof(TFor); }
        }

        public sealed override void Perform(object item)
        {
            Perform((TFor)item);
        }

        public abstract void Perform(TFor node);

        public override string CanPerform(object arg)
        {
            return CanPerform((TFor) arg);
        }

        public sealed override bool IsChecked(object arg)
        {
            var @for = arg as TFor;
            if (@for != null) 
            return IsChecked(@for);
            return false;
        }

        public virtual bool IsChecked(TFor arg)
        {
            return false;
        }

        public abstract string CanPerform(TFor node);

    }
}