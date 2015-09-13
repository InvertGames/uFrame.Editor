namespace Invert.Core.GraphDesigner
{
    public interface IGraphEvents
    {


        void GraphCreated(IProjectRepository project, IGraphData graph);
        void GraphLoaded(IProjectRepository project, IGraphData graph);
        void GraphDeleted(IProjectRepository project, IGraphData graph);

    }
}