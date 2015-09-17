using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Invert.Data;
using Invert.IOC;
using UnityEngine;

namespace Invert.Core.GraphDesigner {
    public class NavigationSystem : DiagramPlugin
        , IExecuteCommand<NavigateToNodeCommand>
       

    {
        public void Execute(NavigateToNodeCommand nodeCommand)
        {

            var graph = nodeCommand.Node.Graph;

            var workspace = WorkspaceService.Workspaces.FirstOrDefault(p => p.Graphs.Any(x => x.Identifier == graph.Identifier));
            WorkspaceService.Execute(new OpenWorkspaceCommand()
            {
                Workspace = workspace
            });
            WorkspaceService.CurrentWorkspace.CurrentGraphId = graph.Identifier;
            var filterPath = nodeCommand.Node.FilterPath().ToArray();
            
            graph.PopToFilter(graph.RootFilter);
            foreach (var item in filterPath)
            {
                if (item == graph.RootFilter) continue;
                graph.PushFilter(item);
            }
            if (nodeCommand.Select)
                nodeCommand.Node.IsSelected = true;


            var graphNode = nodeCommand.Node as GraphNode;
            if (graphNode != null && filterPath.Any())
            {
                var filter = graphNode.FilterLocations.FirstOrDefault(p => p.FilterId == filterPath.Last().Identifier);
                if (filter != null)
                {
                    var position = filter.Position;

                    Execute(new ScrollGraphCommand()
                    {
                        Position = position
                    });

                }
            }
        }

        public override void Loaded(UFrameContainer container)
        {
            base.Loaded(container);
            WorkspaceService = container.Resolve<WorkspaceService>();
            GraphSystem = container.Resolve<GraphSystem>();
            Repository = container.Resolve<IRepository>();
        }

        public IRepository Repository { get; set; }

        public GraphSystem GraphSystem { get; set; }

        public WorkspaceService WorkspaceService { get; set; }
       
    }



    public class ScrollGraphCommand : ICommand
    {
        public Vector2 Position;

        public string Title
        {
            get { return "ScrollTo"; }
            set { }
        }
    }
}
