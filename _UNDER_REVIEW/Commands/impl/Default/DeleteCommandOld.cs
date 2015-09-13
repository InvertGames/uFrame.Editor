using System;
using System.IO;
using System.Linq;
using Invert.Data;

namespace Invert.Core.GraphDesigner
{
    public class DeleteCommandOld : EditorCommand<DiagramNodeViewModel>, IDiagramNodeCommand,IKeyBindable, ICommand
    {
        public override string Group
        {
            get { return "File"; }
        }

        public override decimal Order
        {
            get { return 3; }
        }

        public override bool CanProcessMultiple
        {
            get { return false; }
        }

        public override void Execute(ICommandHandler handler)
        {
            base.Execute(handler);

        }

        public override void Perform(DiagramNodeViewModel node)
        {
            //InvertApplication.Container.Resolve<IRepository>()
            //    .Remove(node.GraphItemObject);
            //var graphConfig = InvertApplication.Container.Resolve<IGraphConfiguration>();
           
            //var generators = node.CodeGenerators.ToArray();

            //var customFiles = generators.Select(p=>p.Filename).ToArray();
            //var customFileFullPaths = generators.Select(p => System.IO.Path.Combine(graphConfig.CodeOutputSystemPath, p.Filename)).Where(File.Exists).ToArray();
            //var customMetaFiles = generators.Select(p => System.IO.Path.Combine(graphConfig.CodeOutputSystemPath, p.Filename) + ".meta").Where(File.Exists).ToArray();

            //if (node.IsFilter)
            //{

            //    if (node.HasFilterItems)
            //    {
            //        var list = node.ContainedItems.Select(p => string.Format("{0} node inside '{1}'graph", p.Name, p.Graph.Name)).ToArray();
            //        InvertGraphEditor.Platform.MessageBox("Delete sub items first.",
            //            "There are items defined inside this item please hide or delete them before removing this item." + Environment.NewLine + string.Join(Environment.NewLine,list), "OK");
            //        return;
            //    }
            //}
            //if (InvertGraphEditor.Platform.MessageBox("Confirm", "Are you sure you want to delete this?", "Yes", "No"))
            //{
            //    InvertGraphEditor.ExecuteCommand(_ =>
            //    {
            //        node.GraphItemObject.Repository.Remove(node.GraphItemObject);
            //    });
                
            //    if (customFileFullPaths.Length > 0)
            //    {
            //        if (InvertGraphEditor.Platform.MessageBox("Confirm",
            //            "You have files associated with this. Delete them too?" + Environment.NewLine +
            //            string.Join(Environment.NewLine, customFiles), "Yes Delete Them", "Don't Delete them"))
            //        {
            //            InvertGraphEditor.ExecuteCommand(_ =>
            //            {
            //                foreach (var customFileFullPath in customFileFullPaths)
            //                {
            //                    File.Delete(customFileFullPath);
            //                }
            //                foreach (var metaFile in customMetaFiles)
            //                {
            //                    File.Delete(metaFile);
            //                }
            //                //var saveCommand = InvertGraphEditor.Container.Resolve<IToolbarCommand>("SaveCommand");
            //                //Execute the save command
            //                InvertGraphEditor.ExecuteCommand(new SaveCommand());
            //            });
            //        }
            //    }
            //}
        }


        public override string CanPerform(DiagramNodeViewModel node)
        {
            var selected = node.DataObject as IDiagramNode;
            if (node.IsCurrentFilter) 
                return "Can't delete while inside of the node.";
            if (selected == null) 
                return "Select something first.";
            //if (!node.IsLocal) 
            //    return "Must be local to delete. Use hide instead.";
            //if (selected.Graph.Identifier != InvertGraphEditor.CurrentDiagramViewModel.GraphData.Identifier)
            //    return "Must be local to delete. Use hide instead.";
            return null;
        }
    }
}