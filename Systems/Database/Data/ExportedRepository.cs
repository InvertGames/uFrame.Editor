using System.Collections.Generic;
using Invert.Json;

namespace Invert.Data
{
    public class ExportedRepository
    {
        [JsonProperty]
        public string Type { get; set; }
        [JsonProperty]
        public List<ExportedRecord> Records { get; set; }
    }
}