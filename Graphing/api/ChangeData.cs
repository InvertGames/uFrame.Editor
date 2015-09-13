using System.Linq;
using Invert.Json;

namespace Invert.Core.GraphDesigner
{
    public abstract class ChangeData : IChangeData
    {
        protected ChangeData()
        {
        }

        public virtual bool IsValid
        {
            get
            {
                return Item.Node.Graph.ChangeData.OfType<GraphItemAdded>().All(p => p.ItemIdentifier != ItemIdentifier);
            }
        }

        protected ChangeData(IDiagramNodeItem item)
        {
            Item = item;
            ItemIdentifier = item.Identifier;
        }

        public virtual void Serialize(JSONClass cls)
        {
            if (Item != null)
            {
                cls.Add("ItemIdentifier", new JSONData(Item.Identifier));
            }
            
        }

        public virtual void Deserialize(JSONClass cls)
        {
            if (cls["ItemIdentifier"] != null)
            {
                ItemIdentifier = cls["ItemIdentifier"].Value;
            }
            
        }

        public IDiagramNodeItem Item { get; set; }
        public string ItemIdentifier { get; set; }
    
        public abstract void Update(IChangeData data);

    }
}