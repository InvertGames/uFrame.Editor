namespace Invert.Core.GraphDesigner
{
    public interface IFlagCommand : IEditorCommand
    {
        UnityEngine.Color Color { get; set; }
        string FlagName { get; }
    }
}