namespace Invert.Core.GraphDesigner
{
    public interface IRemoveWorkspace
    {
        void RemoveWorkspace(string name);
        void RemoveWorkspace(Workspace workspace);
    }
}