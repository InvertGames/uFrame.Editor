using System.Collections.Generic;

namespace Invert.Core.GraphDesigner
{
    public abstract class CrossDiagramCommand : EditorCommand<DiagramNodeViewModel>, IDiagramContextCommand, IDynamicOptionsCommand, IDiagramNodeCommand
    {
        public override void Execute(ICommandHandler handler)
        {

            base.Execute(handler);
            //var node = item as DiagramViewModel;
            //if (node == null) return;

            //var data = node.Data.NodeItems.LastOrDefault();

            //if (data == null) return;
            //data.BeginEditing();
        }

        public override void Perform(DiagramNodeViewModel diagram)
        {
            if (SelectedOption != null)
            {
                var targetDiagram = SelectedOption.Value as IGraphData;
                var sourceDiagram = diagram.DiagramViewModel.GraphData;
                var selectedNode = diagram.GraphItemObject;
                Perform(sourceDiagram, selectedNode, targetDiagram);

                //diagram.CurrentRepository.SetItemLocation(newNodeData, uFrameEditor.CurrentMouseEvent.MouseDownPosition);
                //diagram.AddNode(newNodeData);
            }
        }

        public override string CanPerform(DiagramNodeViewModel node)
        {
            if (node == null) return "Diagram must be loaded first.";
            
            //if (!node.Data.CurrentFilter.IsAllowed(null, typeof(TType)))
            //    return "Item is not allowed in this part of the diagram.";

            return null;
        }

        public override string Path
        {
            get { return "Move To"; }
        }

        public UFContextMenuItem SelectedOption { get; set; }
        public MultiOptionType OptionsType { get; private set; }

        public IEnumerable<UFContextMenuItem> GetOptions(object item)
        {
            var viewModel = item as DiagramNodeViewModel;
            if (viewModel == null) yield break;

            var diagrams = viewModel.GraphItemObject.Repository.AllOf<IGraphData>();

            foreach (var diagram in diagrams)
            {
                yield return new UFContextMenuItem()
                {
                    Name = this.Path + "/ " + diagram.Name,
                    Value = diagram
                };
            }
        }

        protected abstract void Perform(IGraphData sourceDiagram, IDiagramNode selectedNode, IGraphData targetDiagram);
    }
}