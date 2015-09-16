using System;
using System.Linq;
using Invert.Core;
using Invert.Core.GraphDesigner;
using Invert.Data;
using Invert.IOC;

public class GraphSystem : DiagramPlugin
    , IContextMenuQuery
    , IToolbarQuery
    , IExecuteCommand<CreateGraphMenuCommand>
    , IExecuteCommand<CreateGraphCommand>
    , IExecuteCommand<AddGraphToWorkspace>
{
    public override void Loaded(UFrameContainer container)
    {
        base.Loaded(container);
        WorkspaceService = container.Resolve<WorkspaceService>();
    }

    public WorkspaceService WorkspaceService { get; set; }

    public void QueryContextMenu(ContextMenuUI ui, MouseEvent evt, object obj)
    {
        if (obj is CreateGraphMenuCommand)
        {
            var config = WorkspaceService.CurrentConfiguration;
            foreach (var item in config.GraphTypes)
            {
                ui.AddCommand(new ContextMenuItem()
                {
                    Title = item.Title ?? item.GraphType.Name,
                    Command = new CreateGraphCommand()
                    {
                        GraphType = item.GraphType,
                        Name = "New" + item.GraphType.Name
                    }
                });
            }
            
        }
        var diagram = obj as DiagramViewModel;
        if (diagram != null)
        {
            ui.AddSeparator();
            ui.AddCommand(new ContextMenuItem()
            {
                Title = "Delete This Graph",
                Group = "Remove",
                Command =  new LambdaFileSyncCommand("Delete Graph", () =>
                {
                    Container.Resolve<IRepository>().Remove(diagram.DataObject as IDataRecord);
                })
            });
        }
    }

    public void QueryToolbarCommands(ToolbarUI ui)
    {
        //ui.AddCommand(new ToolbarItem()
        //{
        //    Title = "Create Graph",
        //    Position = ToolbarPosition.BottomRight,
        //    Command = new CreateGraphMenuCommand(),
        //    Order = -1
        //});

        //ui.AddCommand(new ToolbarItem()
        //{
        //    Title = "Import Graph",
        //    Position = ToolbarPosition.BottomRight,
        //    Command = new AddGraphToWorkspace()
        //});
    }

    public void Execute(CreateGraphMenuCommand command)
    {
        Signal<IShowContextMenu>(_ => _.Show(null, command));
    }

    public void Execute(AddGraphToWorkspace command)
    {
        var workspaceService = Container.Resolve<WorkspaceService>();
        var repo = Container.Resolve<IRepository>();
        var workspaceGraphs = workspaceService.CurrentWorkspace.Graphs.Select(p => p.Identifier).ToArray();
        var importableGraphs = repo.AllOf<IGraphData>().Where(p => !workspaceGraphs.Contains(p.Identifier));
        InvertGraphEditor.WindowManager.InitItemWindow(importableGraphs, _ =>
        {
            InvertApplication.Execute(new LambdaCommand("Add Graph", () =>
            {
                workspaceService.CurrentWorkspace.AddGraph(_);
            }));
      
        });
    }

    public void Execute(CreateGraphCommand command)
    {
        var workspaceService = Container.Resolve<WorkspaceService>();
        var repo = Container.Resolve<IRepository>();
        var graph = Activator.CreateInstance(command.GraphType) as IGraphData;
        repo.Add(graph);
        graph.Name = command.Name;
        workspaceService.CurrentWorkspace.AddGraph(graph);
        workspaceService.CurrentWorkspace.CurrentGraphId = graph.Identifier;
       
    }
}