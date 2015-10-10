using System.Collections.Generic;
using Invert.Data;
using Invert.Json;

namespace Invert.Core.GraphDesigner
{
    public class uFrameDatabaseConfig : IDataRecord, IGraphConfiguration, IAlwaysGenerate
    {
        private string _codeOutputPath;
        private string _ns;
        public IRepository Repository { get; set; }
        public string Identifier { get; set; }
        public bool Changed { get; set; }
        public IEnumerable<string> ForeignKeys { get { yield break; } }

        public string Title { get; set; }
        public string Description { get; set; }

        [JsonProperty]
        public string CodeOutputPath
        {
            get { return _codeOutputPath; }
            set { this.Changed("CodeOutputPath", ref _codeOutputPath, value); }
        }

        [JsonProperty]
        public string Namespace
        {
            get { return _ns; }
            set { this.Changed("Namespace", ref _ns, value); }
        }

        public string Group { get { return Title; } }
        public string SearchTag { get { return Title; } }
        
        //       [JsonProperty]
        public bool IsCurrent { get; set; }
        public string FullPath { get; set; }
        public IRepository Database { get; set; }
    }
}