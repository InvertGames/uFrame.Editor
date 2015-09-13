using Invert.Data;
using Invert.Json;

namespace Invert.Core.GraphDesigner
{
    public class InputSelectionValue : IDataRecord,IDataRecordRemoved
    {
        private string _nodeId;
        private string _itemId;
        private string _valueId;
        public IRepository Repository { get; set; }

        [JsonProperty]
        public string Identifier { get; set; }

        public bool Changed { get; set; }

        [JsonProperty]
        public string NodeId
        {
            get { return _nodeId; }
            set { this.Changed("NodeId", ref _nodeId, value); }
        }

        [JsonProperty]
        public string ItemId
        {
            get { return _itemId; }
            set { this.Changed("ItemId", ref _itemId, value); }
        }

        [JsonProperty]
        public string ValueId
        {
            get { return _valueId; }
            set { this.Changed("ValueId", ref _valueId, value); }
        }


        public void RecordRemoved(IDataRecord record)
        {
            if (NodeId == record.Identifier || ItemId == record.Identifier || ValueId == record.Identifier)
                Repository.Remove(this);
        }
    }
}