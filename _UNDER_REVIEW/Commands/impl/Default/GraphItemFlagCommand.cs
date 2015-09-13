using System;
using UnityEngine;

namespace Invert.Core.GraphDesigner
{
    public class GraphItemFlagCommand<T> : EditorCommand<T>, IDiagramNodeCommand, IDiagramNodeItemCommand, IFlagCommand where T : class, IDiagramNodeItem
    {
        private string _title;

        public GraphItemFlagCommand(string flagName,string title = null)
        {
            _title = title;
            FlagName = flagName;
        }

        public override string Group
        {
            get { return "Flags"; }
        }

        public override decimal Order
        {
            get { return -1; }
        }

        public override string Name
        {
            get { return string.IsNullOrEmpty(_title) ? FlagName : _title; }
        }

        //public override string Path
        //{
        //    get { return  Name; }
        //}
        public bool IsProperty { get; set; }
        public Func<T, bool> Get { get; set; }
        public Action<T, bool> Set { get; set; }
        public Color Color { get; set; }
        public string FlagName { get; set; }
        
        public override bool IsChecked(T node)
        {
            if (IsProperty)
            {
                return Get(node);
            }
            return node[FlagName];
        }

        public override void Perform(T node)
        {
            if (IsProperty)
            {
                Set(node, !Get(node));
                return;
            }
            node[FlagName] = !node[FlagName];
        }

        public override string CanPerform(T node)
        {
            if (node == null) return "Node is null. Can't perform flag set command.";
            return null;
        }
    }
  
}