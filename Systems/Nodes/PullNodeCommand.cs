namespace Invert.Core.GraphDesigner
{
    public class PullNodeCommand : Command, IFileSyncCommand
    {
        public GraphNode Node { get; set; }
    }
}