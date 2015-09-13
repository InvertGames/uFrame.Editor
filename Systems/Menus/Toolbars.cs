using System;
using Invert.IOC;

namespace Invert.Core.GraphDesigner
{
    public class Toolbars : DiagramPlugin, IToolbarQuery, ICommandExecuted, IChangeDatabase, IWorkspaceChanged
    {
        public override void Initialize(UFrameContainer container)
        {
            base.Initialize(container);
            
        }

        public override void Loaded(UFrameContainer container)
        {
            base.Loaded(container);
            ToolbarUI = container.Resolve<ToolbarUI>();
            Signal<IToolbarQuery>(_ => _.QueryToolbarCommands(ToolbarUI));
        }

        public ToolbarUI ToolbarUI { get; set; }
        public void QueryToolbarCommands(ToolbarUI ui)
        {
            
           
        }
        public void CommandExecuted(ICommand command)
        {
            RefreshToolbar();
        }

        private void RefreshToolbar()
        {
            ToolbarUI.AllCommands.Clear();
            ToolbarUI.LeftCommands.Clear();
            ToolbarUI.RightCommands.Clear();
            ToolbarUI.BottomLeftCommands.Clear();
            ToolbarUI.BottomRightCommands.Clear();
            Signal<IToolbarQuery>(_ => _.QueryToolbarCommands(ToolbarUI));
        }

        public void ChangeDatabase(IGraphConfiguration configuration)
        {
            RefreshToolbar();
        }

        public void WorkspaceChanged(Workspace workspace)
        {
            RefreshToolbar();
        }
    }
}