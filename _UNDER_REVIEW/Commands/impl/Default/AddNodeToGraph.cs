using System;
using System.Collections.Generic;
using System.IO;


namespace Invert.Core.GraphDesigner
{
    public class AddNodeToGraph : EditorCommand<DiagramViewModel>, IDiagramContextCommand,IDynamicOptionsCommand
    {
        public override string Name
        {
            get { return "Add"; }
        }

        public override void Perform(DiagramViewModel node)
        {
            if (SelectedOption != null)
            {
                var newNodeData = Activator.CreateInstance(SelectedOption.Value as Type) as IDiagramNode;
                
                node.AddNode(newNodeData);
                newNodeData.BeginEditing();
            }
        }

        public override string CanPerform(DiagramViewModel node)
        {
            if (node == null) return "Diagram must be loaded first.";

            //if (!node.Data.CurrentFilter.IsAllowed(null, typeof(TType)))
            //    return "Item is not allowed in this part of the diagram.";

            return null;
        }

        public override string Path
        {
            get { return "Add New/" + Title; }
        }

        public IEnumerable<UFContextMenuItem> GetOptions(object item)
        {
            var viewModel = item as DiagramViewModel;
            var filterType = viewModel.GraphData.CurrentFilter.GetType();
            if (!FilterExtensions.AllowedFilterNodes.ContainsKey(filterType))
            {
                InvertApplication.Log(string.Format("The filter type {0} was not find. Make sure the filter is registered, or it has sub nodes for it.", filterType.Name));
                yield break;
            }
            foreach (var nodeType in FilterExtensions.AllowedFilterNodes[filterType])
            {
                if (nodeType.IsAbstract) continue;
                yield return new UFContextMenuItem()
                {
                    Name = "Add " + GetName(nodeType),
                    Value = nodeType
                };
            }
        }

        public string GetName(Type nodeType)
        {
            var config = InvertGraphEditor.Container.Resolve<NodeConfigBase>(nodeType.Name);
            if (config != null)
            {
                return config.Name;
            }
            return nodeType.Name.Replace("Data", "").Replace("Node", "");
        }
        public UFContextMenuItem SelectedOption { get; set; }
        public MultiOptionType OptionsType { get; private set; }
    }

    //public class ToExternalGraph :PullFromCommand
    //{
    //    public override string Name
    //    {
    //        get { return "To External Graph"; }
    //    }
    //    public override string Path
    //    {
    //        get { return "To External Graph"; }
    //    }
    //    protected override void Process(DiagramNode node, IGraphData sourceDiagram, IGraphData targetDiagram)
    //    {
    //       var graph = node.Project.CreateNewDiagram(typeof (InvertGraph), node) as IGraphData;
    //       graph.Name = node.Name;

    //       // MoveNode(node, targetDiagram, graph);
    //        var allchildren = GetAllChildNodes(node).ToArray();
    //        foreach (var item in allchildren)
    //        {
    //            graph.AddNode(item);
    //            Debug.Log(string.Format("moving {0} in graph {1} to graph {2}",  item.Name, targetDiagram.Name, node.Name));
    //        }
    //        foreach (var item in allchildren)
    //        {
    //            targetDiagram.RemoveNode(item);
    //        }
    //        // targetDiagram.RemoveNode(node);

    //    }

    //    private IEnumerable<DiagramNode> GetAllChildNodes(DiagramNode node)
    //    {
    //        foreach (var item in node.GetContainingNodesInProject(node.Project).OfType<DiagramNode>())
    //        {
    //            if (item == node) continue;
    //            if (item.Graph != node.Graph) continue;
    //            //if (item.GetParentNodes().Count() > 1)
    //            //{
    //            //    UnityEngine.Debug.Log(string.Format("Skipping {0} because it is located in more than one place.", item.Name));
    //            //    continue;
    //            //}
    //            yield return item;
    //            foreach (var x in GetAllChildNodes(item))
    //            {
    //                yield return x;
    //            }
    //        }
    //    }
    //    private void MoveNode(DiagramNode node, IGraphData sourceDiagram, IGraphData targetDiagram)
    //    {
    //        foreach (var item in node.GetContainingNodes(sourceDiagram).Where(p=>p.Graph == sourceDiagram).OfType<DiagramNode>())
    //        {
    //            if (item == node) continue; 
    //            MoveNode(item, sourceDiagram, targetDiagram);
    //            var positionData = sourceDiagram.PositionData[node, item];
    //            targetDiagram.AddNode(item);
    //            targetDiagram.PositionData[node, item] = positionData;
    //            sourceDiagram.RemoveNode(item);
               
    //        }
    //    }

    //    public override string CanPerform(DiagramNode node)
    //    {
    //        //if (node.Graph != InvertGraphEditor.DesignerWindow.DiagramViewModel.DiagramData)
    //        //{
    //        //    return "Node must be local.";
    //        //}
    //        return null;
    //    }
    //}
}
