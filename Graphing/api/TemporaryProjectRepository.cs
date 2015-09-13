using System;
using System.Collections.Generic;

namespace Invert.Core.GraphDesigner
{
    /// <summary>
    /// A temporary project repository for manual build processes.
    /// </summary>
    [Obsolete]
    public class TemporaryProjectRepository : DefaultProjectRepository
    {
        public TemporaryProjectRepository(IEnumerable<IGraphData> graphs)
        {
            Graphs = graphs;

        }

        public TemporaryProjectRepository(IGraphData currentGraph, IEnumerable<IGraphData> graphs)
        {
            CurrentGraph = currentGraph;
            Graphs = graphs;
        }

        public TemporaryProjectRepository(IGraphData currentGraph)
        {
            CurrentGraph = currentGraph;
            Graphs = new[] { currentGraph };
        
        }

        public TemporaryProjectRepository()
        {
        }

        public override IGraphData CurrentGraph { get; set; }

        public override IEnumerable<IGraphData> Graphs { get; set; }

        protected override string LastLoadedDiagram { get; set; }

        public override void RecordUndo(INodeRepository data, string title)
        {

        }

        public override void Refresh()
        {

        }

        public override void SaveDiagram(INodeRepository data)
        {

        }



    }
}