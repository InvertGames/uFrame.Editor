using Invert.Json;

namespace Invert.Core.GraphDesigner
{
    public abstract class StringChange : ChangeData
    {
        public string Old { get; set; }
        public string New { get; set; }

        public override bool IsValid
        {
            get { return Old != null && New != null && Old != New && base.IsValid; }
        }

        protected StringChange()
        {
        }

        protected StringChange(IDiagramNodeItem item, string old, string @new)
        {
            Old = old;
            New = @new;
            Item = item;
            ItemIdentifier = item.Identifier;
        }

        
        public override void Serialize(JSONClass cls)
        {
            base.Serialize(cls);
            if (!string.IsNullOrEmpty(Old))
            {
                cls.Add("Old", new JSONData(Old));
            }
            if (!string.IsNullOrEmpty(New))
            {
                cls.Add("New", new JSONData(New));
            }
        }

        public override void Deserialize(JSONClass cls)
        {
            base.Deserialize(cls);
            if (cls["Old"] != null)
            {
                Old = cls["Old"].Value;
            }
            if (cls["New"] != null)
            {
                New = cls["New"].Value;
            }
        }

        
    }
}