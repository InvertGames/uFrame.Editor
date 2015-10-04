using System.Linq;
using Invert.Core;
using Invert.Core.GraphDesigner;
using Invert.Data;
using UnityEditor;
using UnityEngine;

namespace Assets.UnderConstruction.Editor
{
    public class GraphTreeWindow : EditorWindow
    {
        [MenuItem("Window/uFrame/Graph Explorer %T")]
        public static void Init()
        {
            var window = GetWindow<GraphTreeWindow>();
            window.minSize = new Vector2(200, 200);
            window.title = "Graph Explorer";
            window.Show();
            window.Repaint();
            window.Focus();
            Instance = window;
            window.Focused = true;
        }

        public static GraphTreeWindow Instance { get; set; }

        void OnGUI()
        {
            InvertApplication.SignalEvent<IDrawGraphTreeWindow>(_=>_.DrawGraphTreeWindow(this.position.width,this.position.height));
        }

        void Update()
        {
           
            if (Dirty || Focused)
            {
                Repaint();
                Dirty = false;
            }
        }

        public void OnLostFocus()
        {
            Focused = true;
        }

        public void OnFocus()
        {
            Focused = true;
        }
        public bool Focused { get; set; }
        public bool Dirty { get; set; }
    }
    
    public interface IDrawGraphTreeWindow
    {
        void DrawGraphTreeWindow(float width, float height);
    }
    public class GraphTreeWindowPlugin : DiagramPlugin, IGraphSelectionEvents, IDrawGraphTreeWindow, IDataRecordPropertyChanged, IDataRecordRemoved, IDataRecordInserted
    {
        private WorkspaceService _workspaceService;
        private TreeViewModel _treeModel;
        public string SearchCriteria { get; set; }
        private IPlatformDrawer _platrformDrawer;

        public WorkspaceService WorkspaceService
        {
            get { return _workspaceService ?? (_workspaceService = InvertApplication.Container.Resolve<WorkspaceService>()); }
            set { _workspaceService = value; }
        }

        
        public IPlatformDrawer PlatformDrawer
        {
            get { return _platrformDrawer ?? (_platrformDrawer = InvertApplication.Container.Resolve<IPlatformDrawer>()); }
            set { _platrformDrawer = value; }
        }

        public IGraphData GraphData
        {
            get
            {
                return WorkspaceService.CurrentWorkspace == null ? null : WorkspaceService.CurrentWorkspace.CurrentGraph;
            }
        }

        public TreeViewModel TreeModel
        {
            get { return _treeModel ?? (_treeModel = GraphData == null ? null : new TreeViewModel()
            {
                Data = WorkspaceService == null ? null : WorkspaceService.Workspaces.Cast<IItem>().ToList(),
                Submit = TryNavigateToItem
            }); }
            set { _treeModel = value; }
        }


        public GraphItemViewModel LastSelectedGraphItem { get; set; }

        void OnEnable()
        {
            
        }

        private void SetHardDirty() //If you know what I mean
        {
            TreeModel = null;
        }

        private void TryNavigateToItem(IItem item)
        {
            if(InvertGraphEditor.CurrentDiagramViewModel != null)
                InvertGraphEditor.CurrentDiagramViewModel.NothingSelected();

            var itemAsNode = item as IDiagramNodeItem;
            if(itemAsNode !=null ){
                InvertApplication.Execute(new NavigateToNodeCommand()
                    {
                        Node = itemAsNode.Node
                    });
                return;
            }

        }


        public void SelectionChanged(GraphItemViewModel selected)
        {
            if (TreeModel == null) return;
            var selectedData = selected == null ? null : selected.DataObject;

            if (selectedData != TreeModel.SelectedData)
            {
                var item = TreeModel.TreeData.FirstOrDefault(_ => _.Data == selectedData);
                if (item != null)
                {
                    TreeModel.SelectedIndex = item.Index;
                    TreeModel.ExpandPathTo(item);
                    TreeModel.ScrollToItem(item);
                }
                else
                {
                    TreeModel.SelectedIndex = -1;
                }
            }
            Repaint();
        }

        private static void Repaint()
        {
            if (GraphTreeWindow.Instance != null)
            GraphTreeWindow.Instance.Repaint();
        }

        public void DrawGraphTreeWindow(float width, float height)
        {
            if (TreeModel == null) return;
            var window = new Rect(0, 0, width, height);

            var searcbarRect = window.WithHeight(50).Pad(5, 5, 55, 10);
            var listRect = window.Below(searcbarRect).Clip(window).PadSides(5);
            var searchIconRect = new Rect().WithSize(31, 31).AlignHorisonallyByCenter(searcbarRect).RightOf(searcbarRect).Translate(10, 0);

            PlatformDrawer.DrawImage(searchIconRect, "SearchIcon", true);

            EditorGUI.BeginChangeCheck();
            GUI.SetNextControlName("Search");
            SearchCriteria = GUI.TextField(searcbarRect, SearchCriteria ?? "");

            if (EditorGUI.EndChangeCheck())
            {
                if (string.IsNullOrEmpty(SearchCriteria))
                {
                    TreeModel.Predicate = null;
                }
                else
                {
                    TreeModel.Predicate = i => i.Title.Contains(SearchCriteria);
                }
            }

            InvertApplication.SignalEvent<IDrawTreeView>(_ =>
            {
                _.DrawTreeView(listRect, TreeModel, (m, i) =>
                {
                    TryNavigateToItem(i);
                });
            });

            GUI.FocusControl("Search");
        }

        public void PropertyChanged(IDataRecord record, string name, object previousValue, object nextValue)
        {
            Repaint();
        }

        public void RecordRemoved(IDataRecord record)
        {
            Repaint();
        }

        public void RecordInserted(IDataRecord record)
        {
            Repaint();
        }
    }
}