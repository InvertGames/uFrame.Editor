using System.Linq;

namespace Invert.Core.GraphDesigner
{
    public class GraphItemRemoved : ChangeData
    {
        public override bool IsValid
        {
            get
            {
                return this.Item.Node.Graph.ChangeData.OfType<GraphItemAdded>().All(p => p.ItemIdentifier != Item.Identifier);
            }
        }

        public override void Update(IChangeData data)
        {
            
        }

        public override string ToString()
        {
            return Item.Name + " was removed";
        }
    }
}