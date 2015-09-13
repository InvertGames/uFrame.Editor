namespace Invert.Core.GraphDesigner.Data.Upgrading
{
    public interface IUpgradeProcessor
    {
        double Version { get; }

        void Upgrade(INodeRepository repository, IGraphData graphData);
    }

  
}