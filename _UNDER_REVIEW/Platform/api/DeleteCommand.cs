namespace Invert.Core.GraphDesigner
{
    public class DeleteCommand : Command, IFileSyncCommand
    {
        public Invert.Data.IDataRecord Item { get; set; }
    }
}