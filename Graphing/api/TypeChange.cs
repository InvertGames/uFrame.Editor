namespace Invert.Core.GraphDesigner
{
    public class TypeChange : StringChange
    {
        public TypeChange(IDiagramNodeItem item, string old, string @new) : base(item, old, @new)
        {
        }

        public TypeChange()
        {
        }

        public override void Update(IChangeData data)
        {
            var tc = data as TypeChange;
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
            if (Item == null) return string.Format("Type Change {0} - {1}", Old ?? string.Empty, New ?? string.Empty);

            return string.Format("{0}: Type {1} Changed to {2}",Item.Label, Old, New);
        }
    }
}