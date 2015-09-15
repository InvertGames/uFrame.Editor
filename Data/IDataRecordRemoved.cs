namespace Invert.Data
{
    public interface IDataRecordRemoved
    {
        void RecordRemoved(IDataRecord record);
    }

    public interface IDataRecordRemoving
    {
        void RecordRemoving(IDataRecord record);
    }

    public interface IDataRecordManagerRefresh
    {
        void ManagerRefreshed(IDataRecordManager manager);
    }
}