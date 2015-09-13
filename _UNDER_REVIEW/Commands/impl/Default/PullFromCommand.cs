namespace Invert.Core.GraphDesigner
{
    public class PullFromCommand : EditorCommand<GraphNode>,  IDiagramNodeCommand
    {
        public override string Group
        {
            get { return "Moving"; }
        }
         
        public override string Path
        {
            get { return "Pull From"; }
        }

        public override string Name
        {
            get { return "Pull From Command"; }
        }
        
        //protected override void Perform(IElementDesignerData sourceDiagram, IDiagramNode selectedNode,
        //    INodeRepository targetDiagram)
        //{
            
        //}
        public override bool CanProcessMultiple
        {
            get { return false; }
        }

        public override void Perform(GraphNode node)
        {
            // TODO 2.0 Rewrite pull from
            //var sourceDiagram = node.Graph;
            //var targetDiagram = InvertGraphEditor.DesignerWindow.DiagramViewModel.GraphData;
            //if (targetDiagram == null) return;
            //var editableFilesBefore = InvertGraphEditor.GetAllFileGenerators(null, node.Project, false).Where(p=>p.Generators.Any(x=>!x.AlwaysRegenerate)).ToArray();
            
            //Process(node, sourceDiagram, targetDiagram);

            //var editableFilesAfter = InvertGraphEditor.GetAllFileGenerators(null, node.Project, false).Where(p => p.Generators.Any(x => !x.AlwaysRegenerate)).ToArray();
            ////if (editableFilesBefore.Any(p => File.Exists(System.IO.Path.Combine(p.AssetPath, p.Filename))))
            ////{
            ////    InvertGraphEditor.Platform.MessageBox("Move Files",
            ////        "Pulling this item causes some output paths to change. I'm going to move them now.", "OK");
            ////}
            //foreach (var beforeFile in editableFilesBefore)
            //{
            //    var before = beforeFile.Generators.FirstOrDefault();
            //    if (before == null) continue;
            //    foreach (var afterFile in editableFilesAfter)
            //    {
            //        var after = afterFile.Generators.FirstOrDefault();
            //        if (after == null) continue;

            //        if (before.ObjectData == after.ObjectData)
            //        {
                      
            //            var beforeFilename = beforeFile.SystemPath;
            //            var afterFilename = afterFile.SystemPath;
            //            if (beforeFilename == afterFilename) continue; // No change in path
            //            if (System.IO.Path.GetFileName(beforeFilename) != System.IO.Path.GetFileName(afterFilename))
            //                continue; // Filenames aren't the same
            //            //InvertApplication.Log(string.Format("Moving {0} to {1}", beforeFilename, afterFilename));
            //            if (File.Exists(beforeFilename))
            //            {
            //                var dir = System.IO.Path.GetDirectoryName(afterFilename);
            //                if (!Directory.Exists(dir))
            //                {
            //                    Directory.CreateDirectory(dir);
            //                }
            //                File.Move(beforeFilename, afterFilename);
            //            }
                        
            //            var beforeMetaFilename = beforeFilename + ".meta";
            //            var afterMetaFilename = afterFilename + ".meta";

            //            if (File.Exists(beforeMetaFilename))
            //            {
            //                File.Move(beforeMetaFilename, afterMetaFilename);
            //            }
                        
            //        }
            //    }
            //}
           
#if UNITY_EDITOR
       
            UnityEditor.AssetDatabase.Refresh();
#endif

        }

        protected virtual void Process(GraphNode node, IGraphData sourceDiagram, IGraphData targetDiagram)
        {
       
        }

        public override string CanPerform(GraphNode node)
        {
            if (node == null) return "Invalid input";
            if (node.Graph.Identifier == InvertGraphEditor.CurrentDiagramViewModel.GraphData.Identifier)
                return "The node already exist in this graph.";
            //if (node == node.Graph.RootFilter)
            //    return "This node is the main part of a diagram and can't be removed.";
            //if (node.Graph != InvertGraphEditor.DesignerWindow.DiagramViewModel.DiagramData) 
            //    return "The node must be external to pull it.";
            return null;
        }


    }
}