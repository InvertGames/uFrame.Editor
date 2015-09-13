using System;
using System.Collections.Generic;

namespace Invert.Core.GraphDesigner
{
    [Obsolete]
    public interface IProjectRepository : INodeRepository
    {
        IGraphData CurrentGraph { get; set; }

        IEnumerable<IGraphData> Graphs { get; set; }

        IEnumerable<OpenGraph> OpenGraphs { get; }

        Type RepositoryFor { get; }

        void CloseGraph(OpenGraph tab);

        //Dictionary<string, string> GetProjectDiagrams();
        IGraphData CreateNewDiagram(Type diagramType, IGraphFilter defaultFilter = null);

        bool GetSetting(string key, bool def = true);

        IGraphData LoadDiagram(string path);

        void Refresh();

        bool SetSetting(string key, bool value);
        void TrackChange(IChangeData data);
    }
}