using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Invert.Core.GraphDesigner;
using Invert.Core.GraphDesigner;
using Invert.IOC;

namespace Invert.Core.GraphDesigner
{
    //public class BreadCrumbsCommand : ElementsDiagramToolbarCommand, IDynamicOptionsCommand
    //{

    //    public override void Perform(DiagramViewModel node)
    //    {
    //        node.NothingSelected();
    //        node.GraphData.PopToFilterById((string)SelectedOption.Value);
    //    }

    //    public IEnumerable<UFContextMenuItem> GetOptions(object arg)
    //    {
    //        var item = arg as DiagramViewModel;
    //        if (item == null)
    //        {
    //            yield break;
    //        }
            
    //        yield return new UFContextMenuItem()
    //        {
    //            Name = item.GraphData.RootFilter.Name, 
    //            Value = item.GraphData.RootFilter.Identifier, 
    //            Checked = item.GraphData.CurrentFilter == item.GraphData.RootFilter
    //        };
    //        foreach (var filter in item.GraphData.GetFilterPath())
    //        {
    //            yield return new UFContextMenuItem()
    //            {
    //                Name = filter.Name,
    //                Value = filter.Identifier,
    //                Checked = item.GraphData.CurrentFilter == filter
    //            };
    //        }
    //    }

    //    public override ToolbarPosition Position
    //    {
    //        get { return ToolbarPosition.Left; }
    //    }

    //    public UFContextMenuItem SelectedOption { get; set; }
    //    public MultiOptionType OptionsType { get{ return MultiOptionType.Buttons; } }
    //}

    //public class SelectDiagramCommand : ToolbarCommand<DesignerWindow>, IDropDownCommand
    //{
     
    //    public WorkspaceService ProjectService
    //    {
    //        get
    //        {
    //            return InvertGraphEditor.Container.Resolve<WorkspaceService>();
    //        }
    //    }

    //    public override string Name
    //    {
    //        get
    //        {
    //            var ps = ProjectService;
    //            if (ps == null || ps.CurrentWorkspace == null || ps.CurrentWorkspace.CurrentGraph == null)
    //            {
    //                return "Graph: [None]";
    //            }

    //            return string.Format("Graph: {0}", ps.CurrentWorkspace.CurrentGraph.Name);
    //        }
    //    }

    //    public override void Perform(DesignerWindow node)
    //    {
    //        var projectService = InvertGraphEditor.Container.Resolve<WorkspaceService>();
    //        var contextMenu = InvertApplication.Container.Resolve<ContextMenuUI>();
    //        contextMenu.Handler = node;
    //        foreach (var item in projectService.CurrentWorkspace.Graphs.OrderBy(p=>p.Name))
    //        {
    //            IGraphData item1 = item;

    //            var simpleEditorCommand = new SimpleEditorCommand<DesignerWindow>(_ =>
    //            {
    //                projectService.CurrentWorkspace.CurrentGraph = item1;
    //                //InvertApplication.SignalEvent<IOpenWorkspace>(_=>_.OpenWorkspace(item1));
    //                node.SwitchDiagram(item1);
    //            }, item.Name, "Switch");
             
    //            contextMenu.AddCommand(simpleEditorCommand);

    //        }
    //        contextMenu.AddSeparator("");
    //        foreach (var graphType in InvertGraphEditor.Container.Mappings.Where(p => p.From == typeof(IGraphData)))
    //        {
    //            TypeMapping type = graphType;
    //            contextMenu.AddCommand(new SimpleEditorCommand<DesignerWindow>(_ =>
    //            {
    //                var diagram = projectService.CurrentWorkspace.CreateGraph(type.To);
    //                node.SwitchDiagram(diagram);
    //            }, "Create " + type.To.Name,"Create"));
    //        }
    //        //contextMenu.AddSeparator("");
    //        //contextMenu.AddCommand(new SimpleEditorCommand<DesignerWindow>(_ =>
    //        //{
    //        //    projectService.CurrentProject.Refresh();
    //        //}, "Force Refresh", "Refresh"));
    //        contextMenu.Go();
    //    }

    //    public override string CanPerform(DesignerWindow node)
    //    {
    //        if (node.Workspace == null)
    //            return "No project selected.";

    //        return null;
    //    }

    //    public override ToolbarPosition Position
    //    {
    //        get { return ToolbarPosition.Left; }
    //    }
    //}

}