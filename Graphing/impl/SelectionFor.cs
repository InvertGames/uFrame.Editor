using System.Collections.Generic;
using System.Linq;
using Invert.Data;

namespace Invert.Core.GraphDesigner
{
    public class SelectionFor<TFor, TValue> : GenericSlot
        where TValue : InputSelectionValue, new()
        where TFor : class, IValueItem
    {
        public override bool AllowMultipleInputs
        {
            get { return false; }
        }

        public override bool AllowMultipleOutputs
        {
            get { return false; }
        }

        public virtual TFor Item
        {
            get
            {
                //if (typeof (IConnectable).IsAssignableFrom(typeof (TFor)))
                //{
                return this.InputFrom<TFor>() ?? SelectedItem;
                //}
                return SelectedItem;
            }
        }

        public override bool AllowInputs
        {
            get { return typeof (IConnectable).IsAssignableFrom(typeof(TFor)); }
        }

        public override bool AllowSelection
        {
            get { return this.InputFrom<TFor>() == null; }
        }

        protected virtual TFor SelectedItem
        {
            get
            {
                if (SelectedValue == null) return null;
                return GetAllowed().OfType<TFor>().FirstOrDefault(p=>p.Identifier == SelectedValue.ValueId);
            }
        }

        public TValue SelectedValue
        {
            get
            {
                return Repository.All<TValue>().FirstOrDefault(p => p.NodeId == this.NodeId && p.ItemId == this.Identifier);
            }
        }

        public override string SelectedDisplayName
        {
            get
            {
                
                var item = Item;
                if (item == null) return "...";
               
                return ItemDisplayName(item);
            }
        }

        public virtual string ItemDisplayName(TFor item)
        {
            var xItem = item as IDiagramNodeItem;
            if (xItem != null)
            {
                return xItem.Name;
            }
            return item.Identifier;
        }
        public override void SetInput(IValueItem item)
        {
            base.SetInput(item);
            if (item == null)
            {
                if (SelectedValue != null)
                {
                    Repository.Remove(SelectedValue);
                   
                }

                return;
            }
            var selected = SelectedValue;
            if (selected != null)
            {
                selected.ValueId = item.Identifier;
            }
            else
            {


                var selectedItem = new TValue();
                selectedItem.NodeId = this.NodeId;
                selectedItem.ItemId = this.Identifier;
                selectedItem.ValueId = item.Identifier;
                Repository.Add(selectedItem);
            }
            
        }

        public override IEnumerable<IValueItem> GetAllowed()
        {
            yield break;
        }
    }
}