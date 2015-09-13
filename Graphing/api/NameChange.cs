namespace Invert.Core.GraphDesigner
{
    public class NameChange : StringChange
    {
        public NameChange(IDiagramNodeItem item, string old, string @new) : base(item, old, @new)
        {
        }

        public NameChange()
        {
        }

        public override void Update(IChangeData data)
        {
            var tc = data as NameChange;
            if (tc != null)
            {
                if (New == tc.Old)
                {
                    New = tc.New;
                }
            }
        }
        public override string ToString()
        {
            return string.Format("{0}: Name {1} Changed to {2}", Item.Label, Old, New);
        }
    }
}