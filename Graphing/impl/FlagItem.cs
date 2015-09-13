using Invert.Data;
using Invert.Json;

namespace Invert.Core.GraphDesigner
{
    public class FlagItem : IDataRecord, IDataRecordRemoved
    {
        private string _parentIdentifier;
        private string _name;
        public IRepository Repository { get; set; }
        public string Identifier { get; set; }
        public bool Changed { get; set; }

        [JsonProperty]
        public string ParentIdentifier
        {
            get { return _parentIdentifier; }
            set
            {
                
                this.Changed("ParentIdentifier",ref _parentIdentifier,value);
            }
        }

        [JsonProperty]
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                Changed = true;
            }
        }

        public void RecordRemoved(IDataRecord record)
        {
            if (record.Identifier == ParentIdentifier)
            {
                Repository.Remove(this);
            }
        }
    }
}