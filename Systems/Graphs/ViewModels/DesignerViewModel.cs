using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Invert.Core.GraphDesigner
{
    public class DesignerViewModel : ViewModel<Workspace>
    {
        private ObservableCollection<TabViewModel> _tabs;
        private IToolbarCommand[] _allCommands;
        private WorkspaceService _workspaceService;

        public override void DataObjectChanged()
        {
            base.DataObjectChanged();
            //Tabs = Data.OpenGraphs;
        }

        public WorkspaceService WorkspaceService
        {
            get { return _workspaceService ?? (_workspaceService = InvertGraphEditor.Container.Resolve<WorkspaceService>()); }
        }
        public override object DataObject
        {
            get { return WorkspaceService.CurrentWorkspace; }
            set { base.DataObject = value; }
        }

        public TabViewModel CurrentTab { get; set; }

        public IEnumerable<IGraphData> Tabs
        {
            get { return Data.Graphs; }
        }

        public void OpenTab(IGraphData graphData, string[] path = null)
        {
            Data.CurrentGraph = graphData;
        }

        public IToolbarCommand[] AllCommands
        {
            get { return _allCommands ?? (_allCommands = InvertGraphEditor.Container.ResolveAll<IToolbarCommand>().ToArray()); }
            set { _allCommands = value; }
        }
    }
}