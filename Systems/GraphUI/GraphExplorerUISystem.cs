using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Invert.Common;
using Invert.Core;
using Invert.Core.GraphDesigner;
using Invert.Data;
using UnityEditor;
using UnityEngine;

namespace Assets.Plugins.Editor.uFrame.Editor.Systems.GraphUI
{
    public class GraphExplorerUISystem : DiagramPlugin, IDrawGraphExplorer, IDataRecordInserted, IDataRecordRemoved, ICommandExecuted
    {

        private WorkspaceService _workspaceService;
        private TreeViewModel _treeModel;
        public string SearchCriteria { get; set; }
        private IPlatformDrawer _platrformDrawer;
        private bool _hardDirty;

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
                return WorkspaceService.CurrentWorkspace != null ? WorkspaceService.CurrentWorkspace.CurrentGraph : null;
            }
        }

        public TreeViewModel TreeModel
        {
            get
            {
                if (_treeModel == null && GraphData != null)
                {
                    _treeModel = new TreeViewModel()
                        {
                            Submit = TryNavigateToItem,
                            ColorMarkSelector = i =>
                            {
                                var node = i as GraphNode;
                                if (node != null) return node.Color;
                                return null;
                            },
                            IsDirty = true
                        };
                    SetHardDirty();
                }
                return _treeModel;
            }
            set { _treeModel = value; }
        }

        public GraphItemViewModel LastSelectedGraphItem { get; set; }

        private void SetHardDirty() //If you know what I mean
        {
        

            //TreeModel = null;
        }


        public void DrawGraphExplorer(Rect r)
        {
            if (_hardDirty)
            {
                if (TreeModel == null) return;
                if (WorkspaceService == null || WorkspaceService.CurrentWorkspace == null)
                {
                    TreeModel.Data = new List<IItem>();
                }
                else
                {
                    TreeModel.Data = WorkspaceService.CurrentWorkspace.Graphs.Cast<IItem>().ToList();
                    TreeModel.IsDirty = true;
                }
                _hardDirty = false;
            }
            if (TreeModel == null) return;
      
            if (TreeModel.IsDirty) TreeModel.Refresh();
            Rect window = r;

            var searcbarRect = window.WithHeight(32).PadSides(5);
            var listRect = window.Below(searcbarRect).Clip(window).PadSides(5);
            var searchIconRect = new Rect().WithSize(32, 32).InnerAlignWithBottomRight(searcbarRect).AlignHorisonallyByCenter(searcbarRect).PadSides(10);

            PlatformDrawer.DrawImage(searchIconRect, "SearchIcon", true);

            GUI.SetNextControlName("GraphTreeSearch");
            EditorGUI.BeginChangeCheck();
            SearchCriteria = GUI.TextField(searcbarRect, SearchCriteria ?? "", ElementDesignerStyles.SearchBarTextStyle);
            PlatformDrawer.DrawImage(searchIconRect, "SearchIcon", true);
            if (EditorGUI.EndChangeCheck())
            {

                if (string.IsNullOrEmpty(SearchCriteria))
                {
                    TreeModel.Predicate = null;
                }
                else
                {
                    var sc = SearchCriteria.ToLower();
                    TreeModel.Predicate = i =>
                    {
                        if (string.IsNullOrEmpty(i.Title)) return false;

                        if (
                            CultureInfo.CurrentCulture.CompareInfo.IndexOf(i.Title, SearchCriteria,
                                CompareOptions.IgnoreCase) != -1) return true;

                        if (!string.IsNullOrEmpty(i.SearchTag) &&
                            CultureInfo.CurrentCulture.CompareInfo.IndexOf(i.SearchTag, SearchCriteria,
                                CompareOptions.IgnoreCase) != -1) return true;

                        return false;
                    };
                }
                TreeModel.IsDirty = true;
            }


            //            PlatformDrawer.DrawTextbox("GraphTreeWindow_Search",searcbarRect,_searchCriterial,GUI.skin.textField.WithFont("Verdana",15),
            //                (val,submit) =>
            //                {
            //                    _searchCriterial = val;
            //                });


            InvertApplication.SignalEvent<IDrawTreeView>(_ =>
            {
                _.DrawTreeView(listRect, TreeModel, (m, i) =>
                {
                    TryNavigateToItem(i);
                });
            });

            GUI.FocusControl("GraphTreeSearch");
        }

        private void TryNavigateToItem(IItem item)
        {
            if (InvertGraphEditor.CurrentDiagramViewModel != null)
                InvertGraphEditor.CurrentDiagramViewModel.NothingSelected();

            var itemAsNode = item as IDiagramNodeItem;
            if (itemAsNode != null)
            {
                InvertApplication.Execute(new NavigateToNodeCommand()
                {
                    Node = itemAsNode.Node
                });
                return;
            }

        }

//        void Update()
//        {
//            if (TreeModel != null && TreeModel.IsDirty) TreeModel.Refresh();
//            Repaint();
//        }

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
        }


        public void CommandExecuted(ICommand command)
        {
            _hardDirty = true;
        }

        public void RecordInserted(IDataRecord record)
        {
            if (record is IDiagramNodeItem) _hardDirty = true;
        }

        public void RecordRemoved(IDataRecord record)
        {
            if (record is IDiagramNodeItem) _hardDirty = true;
        }



    }

    public interface IDrawGraphExplorer
    {
        void DrawGraphExplorer(Rect r);
    }
}
