namespace Invert.Core.GraphDesigner
{
    public interface IChangeTrackingEvents
    {
        void ChangeOccured(IChangeData data);
    }
}