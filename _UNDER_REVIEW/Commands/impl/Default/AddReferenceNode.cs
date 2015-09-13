using System.Linq;
using UnityEngine;

namespace Invert.Core.GraphDesigner
{
    public class AddReferenceNode : EditorCommand<DiagramViewModel>
    {
      
        public override string Name
        {
            get
            {
                return "Add Type Reference";
            }
        }
        public override void Perform(DiagramViewModel diagram)
        {
    
            InvertGraphEditor.WindowManager.InitItemWindow(InvertApplication.CachedAssemblies.SelectMany(p=>p.GetTypes()).Select(p=>new GraphTypeInfo()
            {
                Name = p.FullName,
                Label = p.Name
            }), _ =>
            {
                var node = new TypeReferenceNode();
                diagram.AddNode(node, new Vector2(15f, 15f));
                node.Name = _.Label;
                node.FullName = _.Name;
            });
            //InvertGraphEditor.WindowManager.InitTypeListWindow(InvertApplication.CachedAssemblies.SelectMany(p=>p=>new GraphTypeInfo()p.DefinedTypes));
            //InvertGraphEditor.WindowManager.TypeInputWindow((g) =>
            //{
            //    var node = new TypeReferenceNode();
            //    diagram.AddNode(node, new Vector2(15f, 15f));
            //    node.Name = g.Name;
                

            //});
        }

        public override string CanPerform(DiagramViewModel node)
        {
            return null;
        }
    }
}