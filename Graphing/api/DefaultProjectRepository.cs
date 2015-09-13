using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Object = System.Object;

namespace Invert.Core.GraphDesigner
{
    [Obsolete]
    public abstract class DefaultProjectRepository
#if UNITY_EDITOR
        : ScriptableObject, IProjectRepository
#else
        : IProjectRepository
#endif
    {
#if UNITY_EDITOR
        public string AssetPath
        {
            get
            {
                return UnityEditor.AssetDatabase.GetAssetPath(this);
            }
        }

        public string AssetDirectory
        {
            get { return System.IO.Path.GetDirectoryName(AssetPath); }
        }
        public string SystemPath
        {
            get { return Path.Combine(Application.dataPath, AssetPath.Substring(7)).Replace("\\", "/").ToLower(); }
            set
            {

            }
        }
#else
        public string SystemPath
        {
            get;set;
        }
#endif
        public string SystemDirectory
        {
            get
            { 
                return Path.GetDirectoryName(SystemPath);
            }
        }
        protected IGraphData _currentGraph;
        protected IDiagramNode[] _nodeItems;

        private static readonly List<IGraphData> _precompiledGraphs = new List<IGraphData>();
        private List<OpenGraph> _openTabs = new List<OpenGraph>();
        private Dictionary<string, bool> _settingsBag;
        private List<IGraphData> _includedGraphs;

        protected IGraphItem[] _allGraphItems;

        public IEnumerable<IGraphData> PrecompiledGraphs
        {
            get
            {
                return _precompiledGraphs;
            }
        }

        /// <summary>
        /// Adds a precompiled graph to projects
        /// </summary>
        /// <param name="diagramType"></param>
        /// <param name="node"></param>
        public static void AddPrecompiledGraph(IGraphData node)
        {

            _precompiledGraphs.Add(node);
        }

        public bool this[string settingsKey, bool def = true]
        {
            get
            {
                if (SettingsBag.ContainsKey(settingsKey))
                {
                    return SettingsBag[settingsKey];
                }
                return def;
            }
            set
            {
                if (SettingsBag.ContainsKey(settingsKey))
                {
                    SettingsBag[settingsKey] = value;
                }
                else
                {
                    SettingsBag.Add(settingsKey, value);
                }
            }
        }

        /// <summary>
        /// Every single graph item that is stored in graphs in the project, Nodes, and their child items
        /// </summary>
        public IEnumerable<IGraphItem> AllGraphItems
        {
            //get {
            //    return from diagram in Graphs where diagram != null && !Object.Equals(diagram, null) from item in diagram.AllGraphItems select item;
            //}
            get
            {
                if (_allGraphItems != null)
                    return _allGraphItems;
                else
                    return _allGraphItems = (Graphs.Where(
                        diagram => diagram != null && !Object.Equals(diagram, null))
                        .SelectMany(diagram => diagram.AllGraphItems)).ToArray();
            }
        }

        /// <summary>
        /// Every single node in the graphs that belong to this project.
        /// </summary>
        public IEnumerable<IDiagramNode> AllNodeItems
        {
            get
            {
                foreach (var item in Graphs)
                {
                    if (item == null || Object.Equals(item, null)) continue;
                    foreach (var node in item.NodeItems)
                    {
                        yield return node;
                    }

                }
            }
        }

        /// <summary>
        /// A wrapper of the current graphs current filter.
        /// </summary>
        public IGraphFilter CurrentFilter
        {
            get
            {
                return CurrentGraph.CurrentFilter;
            }
        }

        /// <summary>
        /// The current graph that is being displayed to the user.
        /// </summary>
        public abstract IGraphData CurrentGraph { get; set; }

        /// <summary>
        /// All of the graphs the belong to the project.
        /// </summary>
        public abstract IEnumerable<IGraphData> Graphs { get; set; }

        /// <summary>
        /// The last loaded diagram of this project
        /// </summary>
        protected abstract string LastLoadedDiagram { get; set; }

        /// <summary>
        /// The name of the project.
        /// </summary>
        public virtual string Name { get; set; }

        //private IDiagramNode[] _NodeItems;
        /// <summary>
        /// The entire list of nodes in all graphs that belong to this project.
        /// </summary>
        public virtual IEnumerable<IDiagramNode> NodeItems
        {
            get { return _nodeItems ?? (_nodeItems = AllNodeItems.ToArray()); }
            //get { return AllNodeItems; }
        }

        /// <summary>
        /// An exposed list of the open tabs as required by the IProjectRepository interface.
        /// </summary>
        public IEnumerable<OpenGraph> OpenGraphs
        {
            get
            {
                return OpenTabs;
            }
        }

        /// <summary>
        /// Our stored list of open graphs if used by the current platform. IE: Unity
        /// </summary>
        protected virtual List<OpenGraph> OpenTabs
        {
            get { return _openTabs ?? (_openTabs = new List<OpenGraph>()); }
            set { _openTabs = value; }
        }

        /// <summary>
        /// This is a wrapper for the current graphs position data, its a requirement of INodeRepository as well.
        /// </summary>
        public FilterPositionData PositionData
        {
            get { return CurrentGraph.PositionData; }
            set { CurrentGraph.PositionData = value; }
        }

        /// <summary>
        /// The Root namespace for this project
        /// </summary>
        public virtual string Namespace { get; set; }

        [Obsolete]
        public virtual Type RepositoryFor
        {
            get { return typeof(IGraphData); }
        }

        /// <summary>
        /// A wrapper for the current graph's settings. Note: this is NOT project settings
        /// </summary>
        public ElementDiagramSettings Settings
        {
            get
            {
                return CurrentGraph.Settings;
            }
        }

        /// <summary>
        /// Settings bag is where all settings are used, Don't use the collection directly,
        /// use the GetSetting and SetSetting methods.
        /// </summary>
        protected Dictionary<string, bool> SettingsBag
        {
            get { return _settingsBag ?? (_settingsBag = new Dictionary<string, bool>()); }
            set { _settingsBag = value; }
        }

        /// <summary>
        /// Adds a child item to a node, this method should be used so that all other graphs, nodes, and node items
        /// can be notified that an item has been added.
        /// </summary>
        /// <param name="item"></param>
        public virtual void AddItem(IDiagramNodeItem item)
        {
            //item.Node = node;
            var node = item.Node;

            // This feels nasty, but for now it works
            item.NodeItemAdded(item);
            item.Node.NodeItemAdded(item);

            node.PersistedItems = node.PersistedItems.Where(p => !p.Precompiled).Concat(new[] { item });

            foreach (var nodeItem in NodeItems)
            {
                nodeItem.NodeItemAdded(item);
            }
            InvalidateCache();
        }

        /// <summary>
        /// Adds a node to the current graph, this should be used above all others so that
        /// other graphs and nodes can be notified that a node has been added and can perform
        /// additional functionality when needed.
        /// </summary>
        /// <param name="data"></param>
        public virtual void AddNode(IDiagramNode data)
        {

            data.GraphId = CurrentGraph.Identifier;
            foreach (var item in NodeItems)
            {
                data.NodeAdded(data);
                foreach (var containedItem in item.PersistedItems)
                {
                    containedItem.NodeAdded(data);
                }
            }

            //CurrentGraph.AddNode(data);
            InvalidateCache();
        }

        
        [Obsolete]
        protected void InvalidateCache()
        {
            _nodeItems = null;
            _allGraphItems = null;
            var removeList = new List<IGraphData>();
            foreach (var graph in Graphs)
            {
                if (graph == null || graph.Equals(null))
                {
                    removeList.Add(graph);
                    continue;
                }
                //graph.Project = this;
            }
            foreach (var graph in removeList)
            {
                RemoveGraph(graph);
            } 
            _nodeItems = null;
            _allGraphItems = null;
        
        }

        /// <summary>
        /// Simply removes a graph from the open graphs list, simiply: Close the tab.
        /// </summary>
        /// <param name="tab">The tab item to close.</param>
        public void CloseGraph(OpenGraph tab)
        {
            OpenTabs.Remove(tab);
        }
        /// <summary>
        /// Creates a new diagram based on the type passed in and adds it to this project.
        /// <example>
        /// CreateNewDiagram(typeof(MyDiagramData), new MyDiagramNode());
        /// </example>
        /// </summary>
        /// <param name="diagramType">The type of diagram to create.</param>
        /// <param name="defaultFilter">The root node or the root filter to use.</param>
        /// <returns>The created graph.</returns>
        public virtual IGraphData CreateNewDiagram(Type diagramType, IGraphFilter defaultFilter = null)
        {
            var graph = Activator.CreateInstance(diagramType) as InvertGraph;
            // Create a unique name
            graph.Name = string.Format("{0}{1}", diagramType.Name, Graphs.Count());
            // Set up the root node if specified
            if (defaultFilter != null)
            {
                graph.RootFilter = defaultFilter;
                defaultFilter.Name = graph.Name;
            }
            else
            {
                graph.RootFilter = graph.CreateDefaultFilter();

            }

            // Send the signal to any listeners that a new a graph has been created
            InvertApplication.SignalEvent<IGraphEvents>(p => p.GraphCreated(this, graph));
            // Add this graph to the project and do any loading necessary
            AddGraph(graph);
            InvalidateCache();
   
            // Go ahead and save this project
            Save();

            // Return the newly created graph
            return graph;
        }

        /// <summary>
        /// Retreives nodes position in the graph based on the current graph and filter.
        /// </summary>
        /// <param name="node">The node to retreive the position for.</param>
        /// <returns>The location of the node.</returns>
        public Vector2 GetItemLocation(IDiagramNode node)
        {
            return CurrentGraph.PositionData[CurrentFilter, node];
        }

        /// <summary>
        /// Gets project a setting by a key.
        /// </summary>
        /// <param name="key">The key of the setting to get.</param>
        /// <param name="def">The default value if the setting does not currently exist.</param>
        /// <returns></returns>
        public bool GetSetting(string key, bool def = true)
        {
            return this[key, def];
        }

        /// <summary>
        /// This will hide a node in it's current filter state
        /// </summary>
        /// <param name="identifier">The identifier of the node to hide.</param>
        public void HideNode(string identifier)
        {
            CurrentGraph.PositionData.Remove(CurrentGraph.CurrentFilter, identifier);
        }

        [Obsolete]
        public virtual IGraphData LoadDiagram(string path)
        {
            var data = InvertGraphEditor.AssetManager.LoadAssetAtPath(path, RepositoryFor) as IGraphData;
            if (data == null)
            {
                return null;
            }
            CurrentGraph = data;
            return data;
        }

        /// <summary>
        /// Marks the project as dirty, meaning it should be removed. This will be remove later
        /// </summary>
        /// <param name="data"></param>
        public virtual void MarkDirty(INodeRepository data)
        {
            InvalidateCache();

        }



        /// <summary>
        /// Records an undo operation, this may be moved to a more reasonable place in the future.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="title"></param>
        public abstract void RecordUndo(INodeRepository data, string title);

        /// <summary>
        /// Reload this project and all the graphs in it.
        /// </summary>
        public abstract void Refresh();

        /// <summary>
        /// Removes and item from a node.  It uses nodeItem's Node property to access the node, then 
        /// removes it from that node, this allows other nodes and items to be notified of it being removed 
        /// and can take any additional actions if necessary.
        /// </summary>
        /// <param name="nodeItem"></param>
        public virtual void RemoveItem(IDiagramNodeItem nodeItem)
        {
            //nodeItem.Node.RemoveItem(nodeItem);


            foreach (var node in NodeItems.ToArray())
            {
                node.NodeItemRemoved(nodeItem);

                foreach (var item in node.PersistedItems.ToArray())
                {
                    item.NodeItemRemoved(nodeItem);
                }
            }
            InvalidateCache();
    
        }

        /// <summary>
        /// Remove a node, anytime you want to remove a node it should be called here, this way the project can properly
        /// notify other graphs and nodes about this node being removed.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="removePositionData"></param>
        public virtual void RemoveNode(IDiagramNode node, bool removePositionData = true)
        {
            _nodeItems = null;
            foreach (var item in node.PersistedItems.ToArray())
            {
                RemoveItem(item);
            }
            
            foreach (var item in NodeItems.ToArray())
            {
                item.NodeRemoved(node);
            }
            InvalidateCache();
           
        }

        /// <summary>
        /// Save this project and each graph in it.
        /// </summary>
        public virtual void Save()
        {
            var removeList = new List<IGraphData>();
            foreach (var graph in Graphs)
            {
                if (graph == null || graph.Equals(null))
                {
                    removeList.Add(graph);
                    continue;
                }

                graph.Save();
            }
            foreach (var item in removeList)
            {
                RemoveGraph(item);
            }
        }

        protected virtual void RemoveGraph(IGraphData item)
        {

        }

        public abstract void SaveDiagram(INodeRepository data);

        /// <summary>
        /// Sets an items location on the current context's position data.
        /// If stepped into a node the position will be set applied to that nodes
        /// position data. Aka the CurrentFilter 
        /// </summary>
        /// <param name="node">The node to set the position for.</param>
        /// <param name="position">The position to set the node at in the current filter.</param>
        public void SetItemLocation(IDiagramNode node, Vector2 position)
        {
            CurrentGraph.PositionData[CurrentFilter, node] = position;
        }

        /// <summary>
        /// A method for setting a setting on this project
        /// </summary>
        /// <param name="key">The Setting key to set</param>
        /// <param name="value">The boolean value of this setting: On/Off</param>
        /// <returns>The value of the setting.</returns>
        public bool SetSetting(string key, bool value)
        {
            return this[key] = value;
        }

        public void TrackChange(IChangeData data)
        {
            InvertApplication.SignalEvent<IChangeTrackingEvents>(_ => _.ChangeOccured(data));
        }

        public IEnumerable<ErrorInfo> Validate()
        {
            foreach (var graph in this.Graphs)
            {
                foreach (var item in graph.Validate())
                {
                    yield return item;
                }
            }
        }

        /// <summary>
        /// Adds a graph to the internal list of graphs in this project.
        /// </summary>
        /// <param name="graphData"></param>
        protected virtual void AddGraph(IGraphData graphData)
        {

            InvalidateCache();
        }
    }
}