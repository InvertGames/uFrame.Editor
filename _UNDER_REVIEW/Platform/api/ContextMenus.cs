namespace Invert.Core.GraphDesigner
{
    public class ContextMenus : DiagramPlugin,
        IShowContextMenu
    {
        public void Show(MouseEvent evt, params object[] objects)
        {
            var ui = InvertApplication.Container.Resolve<ContextMenuUI>();

            foreach (var item in objects)
            {
                var item1 = item;
                Signal<IContextMenuQuery>(_ => _.QueryContextMenu(ui, evt, item1));
            }
            ui.Go();
        }
    }
}