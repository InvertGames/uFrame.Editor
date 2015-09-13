using System.Collections.Generic;
using UnityEngine;

namespace Invert.Core.GraphDesigner
{
    public class ShowItemCommand : EditorCommand<DiagramViewModel>,
        IToolbarCommand, IDiagramContextCommand, IDynamicOptionsCommand
    {
        public override void Perform(DiagramViewModel node)
        {
            var diagramItem = SelectedOption.Value as IDiagramNode;
            
            node.GraphData.CurrentFilter.ShowInFilter(diagramItem,node.LastMouseEvent.MouseDownPosition);
        }

        public override string CanPerform(DiagramViewModel node)
        {
            //if (node == null) return "Designer Data must not be null";
            return null;
        }

        public ToolbarPosition Position
        {
            get
            {
                return ToolbarPosition.Right;
            }
        }

        public IEnumerable<UFContextMenuItem> GetOptions(object item)
        {
    
            var designerData = item as DiagramViewModel;
            foreach (var importable in designerData.GetImportableItems())
            {
                yield return new UFContextMenuItem()
                {
                    Name = string.Format("Show Item/{0}/{1}", importable.Graph.Name, importable.Name),
                    Value = importable
                };
            }

        }

        public UFContextMenuItem SelectedOption { get; set; }

        public MultiOptionType OptionsType
        {
            get
            {
                return MultiOptionType.DropDown;
            }
        }
    }
}