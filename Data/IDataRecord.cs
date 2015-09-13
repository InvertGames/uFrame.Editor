namespace Invert.Data
{
    public interface IDataRecord : IValueItem
    {
        IRepository Repository { get; set; }
        
        bool Changed { get; set; }
    }
}