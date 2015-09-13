using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Invert.Data;
using Invert.Json;
using uFrame.Serialization;

namespace Invert.Core.GraphDesigner
{
    public interface IGraphData : IElementFileData,IItem, IDataRecord
    {
        List<IChangeData> ChangeData { get; set; }
        string Identifier { get; set; }

        int RefactorCount { get; set; }
    
        string Version { get; set; }

        // Filters
        IGraphFilter RootFilter { get; set; }

        bool Errors { get; set; }
        Exception Error { get; set; }

        bool Precompiled { get; set; }

        string Directory { get;  }
        IGraphFilter[] FilterStack { get; set; }


        //IEnumerable<ConnectionData> Connections { get; }
        void AddConnection(IConnectable output, IConnectable input);
        void AddConnection(string output, string input);
        void RemoveConnection(IConnectable output, IConnectable input);
        void ClearOutput(IConnectable output);
        void ClearInput(IConnectable input); 
       // void SetProject(IProjectRepository value);
        void DeserializeFromJson(JSONNode graphData);
        IGraphFilter CreateDefaultFilter();
        JSONNode Serialize();
        void Deserialize(string jsonData);
        void CleanUpDuplicates();
     
        void PushFilter(IGraphFilter filter);
        void PopToFilter(IGraphFilter filter1);
        void PopToFilterById( string filterId);
        void PopFilter();
    }
}