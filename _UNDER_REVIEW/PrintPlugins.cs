using System;
using System.CodeDom;
using System.Linq;
using System.Reflection;
using Invert.Core.GraphDesigner;
using UnityEngine;

namespace Invert.Core.GraphDesigner
{
    public class PrintPlugins : ElementsDiagramToolbarCommand
    {
        public override string Name
        {
            get { return "Print Json"; }
        }

        public override void Perform(DiagramViewModel node)
        {
            //foreach (var item in node.CurrentRepository.NodeItems)
            //{
            //    Debug.Log(item.Name);
            //}
           
            Type T = typeof(GUIUtility);
            PropertyInfo systemCopyBufferProperty = T.GetProperty("systemCopyBuffer", BindingFlags.Static | BindingFlags.NonPublic);
            
            systemCopyBufferProperty.SetValue(null, InvertGraph.Serialize(node.GraphData).ToString(), null);
            Debug.Log("Json copied to clipboard.");
        }
    }
   
}