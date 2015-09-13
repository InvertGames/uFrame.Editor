using System.Collections.Generic;

namespace Invert.Core.GraphDesigner
{
    public class AddNewCommand : ElementsDiagramToolbarCommand, IParentCommand,IDiagramContextCommand
    {
        public override string Title
        {
            get { return "Add New"; }
        }

        public override void Perform(DiagramViewModel node)
        {
            // No implementation
        }
    }

}