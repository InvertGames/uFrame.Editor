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
        IExecuteCommand<MoveItemUpCommand>,
        IExecuteCommand<MoveItemDownCommand>,
        IOnMouseUpEvent
  
        
    {

        public void QueryContextMenu(ContextMenuUI ui, MouseEvent evt, params object[] objs)
        {
            var diagramNodeItem = objs.FirstOrDefault() as ItemViewModel;
            if (diagramNodeItem != null)
            {
                var item = diagramNodeItem.DataObject as IDiagramNodeItem;
                if (item != null)
                {
                    ui.AddCommand(new ContextMenuItem()
                    {
                        Title = "Move Up",
                        Command = new MoveItemUpCommand()
                        {
                            Item = item
                        }
                    });
                    ui.AddCommand(new ContextMenuItem()
                    {
                        Title = "Move Down",
                        Command = new MoveItemDownCommand()
                        {
                            Item = item
                        }
                    });
                }
            }
            var diagramNode = objs.FirstOrDefault() as DiagramNodeViewModel;
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
                    Command = new HideCommand()
                    {
                        Node = objs.OfType<DiagramNodeViewModel>().Select(p=>p.GraphItemObject).ToArray(),
                        Filter = diagramNode.DiagramViewModel.GraphData.CurrentFilter
                    }
                });
                ui.AddCommand(new ContextMenuItem()
                {
                    Title = "Delete",
                    Group = "Careful",
                    Command = new DeleteCommand()
                    {
                        Item = objs.OfType<DiagramNodeViewModel>().Select(p => p.GraphItemObject).ToArray()
                    }
                });
                if (diagramNode.IsExternal)
                {
                    ui.AddCommand(new ContextMenuItem()
                    {
                        Title = "Pull",
                        Group = "Node",
                        Command = new PullNodeCommand()
                        {
                            Node = objs.OfType<DiagramNodeViewModel>().Select(p => p.GraphItemObject).ToArray(),
                        }
                    });
                }

            }

            var diagram = objs.FirstOrDefault() as DiagramViewModel;
            if (diagram != null)
            {

                var filter = diagram.GraphData.CurrentFilter;
                foreach (var nodeType in FilterExtensions.AllowedFilterNodes[filter.GetType()].OrderBy(p=>p.FullName))
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
#if DEMO
            if (typeof (IDemoVersionLimitZero).IsAssignableFrom(command.NodeType))
            {
                Signal<INotify>(_ => _.NotifyWithActions("You've reached the max number of nodes of this type, upgrade to full version.", NotificationIcon.Warning, new NotifyActionItem()
                {
                    Title = "Buy Now",
                    Action = () =>
                    {
                        InvertGraphEditor.Platform.OpenLink("https://www.assetstore.unity3d.com/en/#!/content/46297");
                    }
                }));
                return;
            }
            if (typeof(IDemoVersionLimit).IsAssignableFrom(command.NodeType))
            {
                var nodeCount = command.GraphData.Repository.AllOf<IDiagramNode>().Count(p => p.GetType() == command.NodeType);
                if (nodeCount >= 7)
                {
                    Signal<INotify>(_ => _.NotifyWithActions("You've reached the max number of nodes of this type, upgrade to full version.", NotificationIcon.Warning, new NotifyActionItem()
                    {
                        Title = "Buy Now",
                        Action = () =>
                        {
                            InvertGraphEditor.Platform.OpenLink("https://www.assetstore.unity3d.com/en/#!/content/46297");
                        }
                    }));
                    return;
                }
            }
           
#endif

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
            foreach (var item in command.Item)
                item.Repository.Remove(item);
        }

        public void Execute(ShowCommand command)
        {
            command.Filter.ShowInFilter(command.Node, command.Position);
        }

        public void Execute(HideCommand command)
        {
            foreach(var item in command.Node)
            command.Filter.HideInFilter(item);
        }

        public void Execute(PullNodeCommand command)
        {
            var workspaceService = Container.Resolve<WorkspaceService>();

            if (workspaceService != null && workspaceService.CurrentWorkspace != null)
            {
                foreach (var gn in command.Node.OfType<GraphNode>())
                {
                    gn.GraphId = workspaceService.CurrentWorkspace.CurrentGraphId;
                    foreach (var item in gn.FilterNodes)
                    {
                        if (gn == item) continue;
                        item.GraphId = workspaceService.CurrentWorkspace.CurrentGraphId;
                    }
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

        public void Execute(MoveItemUpCommand command)
        {
            var items = command.Item.Node.PersistedItems.Where(p => p.GetType() == command.Item.GetType()).ToArray().ToList();
            var index = items.IndexOf(command.Item);
            items.Move(index,true);
            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];
                item.Order = i;
            }
        }

        public void Execute(MoveItemDownCommand command)
        {
            var items = command.Item.Node.PersistedItems.Where(p => p.GetType() == command.Item.GetType()).ToArray().ToList();
            var index = items.IndexOf(command.Item);
            items.Move(index, false);
            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];
                item.Order = i;
            }
        }

	
    }
}
