namespace Invert.Core
{
    public interface IItem
    {
        string Title { get; }
        string Group { get; }
        string SearchTag { get; }
        string Description { get; set; }
    }
}