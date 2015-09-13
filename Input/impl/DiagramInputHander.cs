using System.Linq;

namespace Invert.Core.GraphDesigner
{
    public class DiagramInputHander : IInputHandler
    {
        public GraphItemViewModel ViewModelAtMouse { get; set; }
        public DiagramViewModel DiagramViewModel { get; set; }

        public DiagramInputHander(DiagramViewModel diagramViewModel)
        {
            DiagramViewModel = diagramViewModel;
        }

        public virtual void OnMouseDoubleClick(MouseEvent e)
        {

        }

        public virtual void OnMouseDown(MouseEvent e)
        {

        }

        public virtual void OnMouseMove(MouseEvent e)
        {
            ViewModelAtMouse = DiagramViewModel.GraphItems.Reverse().FirstOrDefault(p => p.Bounds.Contains(e.MousePosition));


        }

        public virtual void OnMouseUp(MouseEvent e)
        {

        }

        public void OnRightClick(MouseEvent mouseEvent)
        {

        }

        public virtual void Draw(IPlatformDrawer platform, float scale)
        {
            
        }
    }
}