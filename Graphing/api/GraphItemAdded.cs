using System.Linq;

namespace Invert.Core.GraphDesigner
{
    public class GraphItemAdded : ChangeData
    {
        public override bool IsValid
        {
            get { return this.Item.Node.Graph.ChangeData.OfType<GraphItemRemoved>().All(p => p.ItemIdentifier != Item.Identifier); }
        }

        public override void Update(IChangeData data)
        {
            
        }

        public override string ToString()
        {
            return Item.Name + " was added";
        }
    }
}