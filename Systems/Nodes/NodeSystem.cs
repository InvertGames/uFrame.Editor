using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Invert.Data;

namespace Invert.Core.GraphDesigner
{
    public class NodeSystem : DiagramPlugin,
        IContextMenuQuery,
        IExecuteCommand<CreateNodeCommand>,
        IExecuteCommand<RenameCommand>,
        IExecuteCommand<DeleteCommand>,
        IExecuteCommand<ShowCommand>,
        IExecuteCommand<HideCommand>,
        IExecuteCommand<PullNodeCommand>,
        IExecuteCommand<ApplyRenameCommand>,
        IOnMouseUpEvent
    {

        public void QueryContextMenu(ContextMenuUI ui, MouseEvent evt, object obj)
        {
            var diagramNode = obj as DiagramNodeViewModel;
            if (diagramNode != null)
            {
                ui.AddCommand(new ContextMenuItem()
                {
                    Title = "Rename",
                    Group = "Node",
                    Command = new RenameCommand() { ViewModel = diagramNode }
                });
                ui.AddCommand(new ContextMenuItem()
                {
                    Title = "Hide",
                    Group = "Node",
                    Command = new HideCommand() { Node = diagramNode.GraphItemObject, Filter = diagramNode.DiagramViewModel.GraphData.CurrentFilter }
                });
                ui.AddCommand(new ContextMenuItem()
                {
                    Title = "Delete",
                    Group = "Careful",
                    Command = new DeleteCommand() { Item = diagramNode.GraphItemObject as Invert.Data.IDataRecord }
                });
                if (diagramNode.IsExternal)
                {
                    ui.AddCommand(new ContextMenuItem()
                    {
                        Title = "Pull",
                        Group = "Node",
                        Command = new PullNodeCommand() { Node = diagramNode.GraphItemObject as GraphNode }
                    });
                }
                
            }

            var diagram = obj as DiagramViewModel;
            if (diagram != null)
            {
                
                var filter = diagram.GraphData.CurrentFilter;
                foreach (var nodeType in FilterExtensions.AllowedFilterNodes[filter.GetType()])
                {
                    if (nodeType.IsAbstract) continue;
                    var config = Container.GetNodeConfig(nodeType);
                    if (config.AllowAddingInMenu)
                    {
                        ui.AddCommand(new ContextMenuItem()
                        {
                            Title = "Create " + Container.GetNodeConfig(nodeType).Name,
                            Group = "Create",
                            Command = new CreateNodeCommand()
                            {
                                NodeType = nodeType,
                                GraphData = diagram.GraphData,
                                Position = diagram.LastMouseEvent.MouseDownPosition
                            }
                        });
                        
                    }
                   
                }
            
                if (filter.AllowExternalNodes)
                {
                   
                    foreach (var item in filter.GetAllowedDiagramItems().OfType<GenericNode>().OrderBy(p => p.Name))
                    {
                        ui.AddCommand(new ContextMenuItem()
                        {
                            Title = "Show/" + item.Config.Name + "/" + item.Name,
                            Group = "Show",
                            Command = new ShowCommand() { Node = item, Filter = filter, Position = evt.MousePosition }
                        });
                    }
                }
                
            }

        }

        public void Execute(CreateNodeCommand command)
        {

            var node = Activator.CreateInstance(command.NodeType) as IDiagramNode;
            var repository = Container.Resolve<IRepository>();
            node.GraphId = command.GraphData.Identifier;
            repository.Add(node);
            if (string.IsNullOrEmpty(node.Name))
                node.Name =
                    repository.GetUniqueName("New" + node.GetType().Name.Replace("Data", ""));
       
            var filterItem = node as IFilterItem;
            if (filterItem != null)
            {
                filterItem.FilterId = command.GraphData.CurrentFilter.Identifier;
                filterItem.Position = command.Position;
            }
            else
            {
                command.GraphData.CurrentFilter.ShowInFilter(node, command.Position);
            }
            
            

        }

        public void Execute(RenameCommand command)
        {
            command.ViewModel.BeginEditing();
        }

        public void Execute(DeleteCommand command)
        {
            command.Item.Repository.Remove(command.Item);
        }

        public void Execute(ShowCommand command)
        {
            command.Filter.ShowInFilter(command.Node, command.Position);
        }

        public void Execute(HideCommand command)
        {
            command.Filter.HideInFilter(command.Node);
        }

        public void Execute(PullNodeCommand command)
        {
            var workspaceService = Container.Resolve<WorkspaceService>();
            if (workspaceService != null && workspaceService.CurrentWorkspace != null)
            {
                command.Node.GraphId = workspaceService.CurrentWorkspace.CurrentGraphId;
                foreach (var item in command.Node.FilterNodes)
                {
                    if (item == command.Node) continue;
                    item.GraphId = workspaceService.CurrentWorkspace.CurrentGraphId;
                }
                
            }
            
        }

        public void OnMouseUp(Drawer drawer, MouseEvent mouseEvent)
        {
            Container.Resolve<IRepository>().Commit();
        }

        public void Execute(ApplyRenameCommand command)
        {
            if (string.IsNullOrEmpty(command.Item.Name))
            {
                command.Item.Name = "RenameMe";
            }
            
            command.Item.Rename(command.Name);
            command.Item.EndEditing();
        }
    }
}
